﻿using System.Collections.Generic;
using System.Linq;
using Martiscoin.Consensus.ScriptInfo;
using Martiscoin.Consensus.TransactionInfo;
using Martiscoin.NBitcoin.BitcoinCore;
using Martiscoin.Networks;

namespace Martiscoin.Features.PoA.Policies
{
    /// <summary>
    /// PoA-specific standard transaction definitions.
    /// </summary>
    public class PoAStandardScriptsRegistry : StandardScriptsRegistry
    {
        // No legacy clients exist for this network with the constraint of 40 bytes
        public const int MaxOpReturnRelay = 83;

        private readonly List<ScriptTemplate> standardTemplates = new List<ScriptTemplate>
        {
            PayToPubkeyHashTemplate.Instance,
            PayToPubkeyTemplate.Instance,
            PayToScriptHashTemplate.Instance,
            PayToMultiSigTemplate.Instance,
            new TxNullDataTemplate(MaxOpReturnRelay),
            PayToWitTemplate.Instance
        };

        public override List<ScriptTemplate> GetScriptTemplates => this.standardTemplates;

        public override void RegisterStandardScriptTemplate(ScriptTemplate scriptTemplate)
        {
            if (!this.standardTemplates.Any(template => (template.Type == scriptTemplate.Type)))
            {
                this.standardTemplates.Add(scriptTemplate);
            }
        }

        public override bool IsStandardTransaction(Transaction tx, Network network)
        {
            return base.IsStandardTransaction(tx, network);
        }

        public override bool AreOutputsStandard(Network network, Transaction tx)
        {
            return base.AreOutputsStandard(network, tx);
        }

        public override ScriptTemplate GetTemplateFromScriptPubKey(Script script)
        {
            return this.standardTemplates.FirstOrDefault(t => t.CheckScriptPubKey(script));
        }

        public override bool IsStandardScriptPubKey(Network network, Script scriptPubKey)
        {
            return base.IsStandardScriptPubKey(network, scriptPubKey);
        }

        public override bool AreInputsStandard(Network network, Transaction tx, CoinsView coinsView)
        {
            return base.AreInputsStandard(network, tx, coinsView);
        }
    }
}