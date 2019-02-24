using Andoromeda.Framework.EosNode;
using Andoromeda.Framework.Logger;
using EosSharp;
using EosSharp.Core;
using Microsoft.Extensions.Configuration;
using Pomelo.AspNetCore.TimedJob;
using smart_signature_timer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_signature_timer.Jobs
{
    public class SyncJob : Job
    {
        static Eos eos = new Eos(new EosConfigurator()
        {
            HttpEndpoint = "http://eos.greymass.com", //Mainnet
            ChainId = "aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906",
            ExpireSeconds = 60,
        });

        [Invoke(Begin = "2018-06-01", Interval = 1000 * 1, SkipWhileExecuting = true)]
        public void SyncActions(IConfiguration config, SSPContext db, ILogger logger)
        {
            var action_pos_Row = db.Constants.FirstOrDefault(x => x.Id == "action_pos");
            var seq = Convert.ToInt32(action_pos_Row.Value);
            //logger.LogInfo($"Current seq is " + seq);

            var actionsResult = eos.GetActions("signature.bp", seq + 1, 1 - 1).Result;
            if (actionsResult.actions != null && actionsResult.actions.Count() != 0)
            {
                for (var i = 0; i < actionsResult.actions.Count; i++)
                {
                    var act = actionsResult.actions[i];

                    try
                    {
                        switch (act.action_trace.act.name)
                        {
                            case "publish":
                                {
                                    var tb_rows_pos_Row = db.Constants.FirstOrDefault(x => x.Id == "tb_rows_pos");
                                    var tbRowsSeq = Convert.ToInt32(tb_rows_pos_Row.Value);

                                    var rowsResult = eos.GetTableRows<PublishRow>(new EosSharp.Core.Api.v1.GetTableRowsRequest()
                                    {
                                        code = "signature.bp",
                                        scope = "signature.bp",
                                        table = "signs",
                                        lower_bound = tbRowsSeq.ToString(),
                                        limit = 1,
                                        json = true
                                    }).Result;

                                    var dbRow = db.Ariticle.FirstOrDefault(x => x.TransactionId == act.action_trace.trx_id);
                                    var chainTbRow = rowsResult.rows.FirstOrDefault();

                                    if (dbRow == null)
                                    {
                                        if (act.block_time <= DateTime.UtcNow.AddMinutes(-1))
                                        {
                                            action_pos_Row.Value = act.account_action_seq.ToString();
                                            tb_rows_pos_Row.Value = (++tbRowsSeq).ToString();
                                        }
                                    }
                                    else
                                    {
                                        dbRow.SignId = chainTbRow.id.ToString();
                                        dbRow.State = 1;
                                        dbRow.FissionFactor = chainTbRow.fission_factor;

                                        action_pos_Row.Value = act.account_action_seq.ToString();
                                        tb_rows_pos_Row.Value = (++tbRowsSeq).ToString();
                                    }
                                }
                                break;
                            default:
                                {
                                    action_pos_Row.Value = act.account_action_seq.ToString();
                                }
                                continue;
                        }
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.ToString());
                    }
                }
            }
        }
    }
}
