using System;
using System.Collections.Generic;
using System.Linq;
using XOuranos.Base.Deployments;
using XOuranos.Consensus;
using XOuranos.Consensus.Checkpoints;
using XOuranos.Features.Consensus.Rules.CommonRules;
using XOuranos.Features.Consensus.Rules.ProvenHeaderRules;
using XOuranos.Features.Consensus.Rules.UtxosetRules;
using XOuranos.Features.MemoryPool.Rules;
using XOuranos.NBitcoin;
using XOuranos.NBitcoin.BouncyCastle.math;
using XOuranos.NBitcoin.DataEncoders;
using XOuranos.NBitcoin.Protocol;
using XOuranos.X1.Consensus;
using XOuranos.X1.Deployments;
using XOuranos.X1.Policies;
using XOuranos.X1.Rules;
using XOuranos.P2P;
using XOuranos.Networks;

namespace XOuranos.X1
{
    public class X1Main : Network
    {
        /// <summary>
        ///     An absolute (flat) minimum fee per transaction, independent of the transaction
        ///     size in bytes or weight. Transactions with a lower fees will be rejected,
        ///     transactions with equal or higher fees are allowed. This property
        ///     Will not be used if the value is null.
        /// </summary>
        public long? AbsoluteMinTxFee { get; protected set; }

        public X1Main()
        {
            this.Name = "OURMain";
            this.NetworkType = NetworkType.Mainnet;
            this.CoinTicker = "OUR";
            this.RootFolderName = "";
            this.DefaultConfigFilename = "OUR.conf";
            this.Magic = 0x58315831; // 
            this.DefaultPort = 23333; // new
            this.DefaultRPCPort = 43333; // new 
            this.DefaultAPIPort = 43334; // new
            this.DefaultMaxOutboundConnections = 16;
            this.DefaultMaxInboundConnections = 109;
            this.MaxTimeOffsetSeconds = 25 * 60;
            this.DefaultBanTimeSeconds = 8000;
            this.MaxTipAge = 48 * 60 * 60;

            this.MinTxFee = Money.Coins(0.00001m).Satoshi;
            this.MaxTxFee = Money.Coins(1).Satoshi;
            this.FallbackFee = Money.Coins(0.00001m).Satoshi;
            this.MinRelayTxFee = Money.Coins(0.00001m).Satoshi;
            this.AbsoluteMinTxFee = Money.Coins(0.00001m).Satoshi;

            var consensusFactory = new X1ConsensusFactory();
            this.GenesisTime = Utils.DateTimeToUnixTime(new DateTime(2020, 1, 2, 23, 56, 00, DateTimeKind.Utc));
            this.GenesisNonce = 15118976;
            this.GenesisBits = new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"));
            this.GenesisVersion = 1;
            this.GenesisReward = Money.Zero;
            this.Genesis = consensusFactory.ComputeGenesisBlock(this.GenesisTime, this.GenesisNonce, this.GenesisBits, this.GenesisVersion, this.GenesisReward, NetworkType.Mainnet);

            var consensusOptions = new X1ConsensusOptions(this)
            {
                MaxBlockBaseSize = 1_000_000,
                MaxStandardVersion = 2,
                MaxStandardTxWeight = 100_000,
                MaxBlockSigopsCost = 20_000,
                MaxStandardTxSigopsCost = 20_000 / 5,
                WitnessScaleFactor = 4
            };

            var buriedDeployments = new BuriedDeploymentsArray
            {
                [BuriedDeployments.BIP34] = 0,
                [BuriedDeployments.BIP65] = 0,
                [BuriedDeployments.BIP66] = 0
            };

            var bip9Deployments = new X1BIP9Deployments
            {
                [X1BIP9Deployments.ColdStaking] = new BIP9DeploymentsParameters("ColdStaking", 27, BIP9DeploymentsParameters.AlwaysActive, 999999999, BIP9DeploymentsParameters.AlwaysActive),
                [X1BIP9Deployments.CSV] = new BIP9DeploymentsParameters("CSV", 0, BIP9DeploymentsParameters.AlwaysActive, 999999999, BIP9DeploymentsParameters.AlwaysActive),
                [X1BIP9Deployments.Segwit] = new BIP9DeploymentsParameters("Segwit", 1, BIP9DeploymentsParameters.AlwaysActive, 999999999, BIP9DeploymentsParameters.AlwaysActive)
            };

            consensusFactory.Protocol = new ConsensusProtocol()
            {
                ProtocolVersion = ProtocolVersion.FEEFILTER_VERSION,
                MinProtocolVersion = ProtocolVersion.POS_PROTOCOL_VERSION,
            };

            this.Consensus = new XOuranos.Consensus.Consensus(
                consensusFactory: consensusFactory,
                consensusOptions: consensusOptions,
                coinType: (int)this.GenesisNonce,
                hashGenesisBlock: this.Genesis.GetHash(),
                subsidyHalvingInterval: 2_100_000,
                majorityEnforceBlockUpgrade: 750,
                majorityRejectBlockOutdated: 950,
                majorityWindow: 1000,
                buriedDeployments: buriedDeployments,
                bip9Deployments: bip9Deployments,
                bip34Hash: this.Genesis.GetHash(),
                minerConfirmationWindow: 2016,
                maxReorgLength: 125,
                defaultAssumeValid: uint256.Zero,
                maxMoney: (long)100_000_000*100_000_000,
                coinbaseMaturity: 5,
                premineHeight: 0,
                premineReward: Money.Coins(100_000),
                proofOfWorkReward: Money.Coins(5),
                targetTimespan: TimeSpan.FromSeconds(30 * 338),
                targetSpacing: TimeSpan.FromSeconds(30),
                powAllowMinDifficultyBlocks: false,
                posNoRetargeting: false,
                powNoRetargeting: false,
                powLimit: new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
                minimumChainWork: null,
                isProofOfStake: true,
                lastPowBlock: 1_000_000_000,
                proofOfStakeLimit: new BigInteger(uint256.Parse("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
                proofOfStakeLimitV2: new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
                proofOfStakeReward: Money.Coins(5),
                proofOfStakeTimestampMask: 0x0000003F // 64 sec
            );

            this.StandardScriptsRegistry = new X1StandardScriptsRegistry();

            this.Base58Prefixes = new byte[12][];
            this.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS] = new byte[] { 0 }; // deprecated - bech32/P2WPKH is used instead
            this.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS] = new byte[] { 5 }; // deprecated - bech32/P2WSH is used instead
            this.Base58Prefixes[(int)Base58Type.SECRET_KEY] = new byte[] { 128 };
            this.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_NO_EC] = new byte[] { 0x01, 0x42 };
            this.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_EC] = new byte[] { 0x01, 0x43 };
            this.Base58Prefixes[(int)Base58Type.EXT_PUBLIC_KEY] = new byte[] { 0x04, 0x88, 0xB2, 0x1E };
            this.Base58Prefixes[(int)Base58Type.EXT_SECRET_KEY] = new byte[] { 0x04, 0x88, 0xAD, 0xE4 };
            this.Base58Prefixes[(int)Base58Type.PASSPHRASE_CODE] = new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2 };
            this.Base58Prefixes[(int)Base58Type.CONFIRMATION_CODE] = new byte[] { 0x64, 0x3B, 0xF6, 0xA8, 0x9A };
            this.Base58Prefixes[(int)Base58Type.ASSET_ID] = new byte[] { 23 };

            var encoder = new Bech32Encoder(this.CoinTicker.ToLowerInvariant());
            this.Bech32Encoders = new Bech32Encoder[2];
            this.Bech32Encoders[(int)Bech32Type.WITNESS_PUBKEY_ADDRESS] = encoder;
            this.Bech32Encoders[(int)Bech32Type.WITNESS_SCRIPT_ADDRESS] = encoder;

            this.Checkpoints = new Dictionary<int, CheckpointInfo>();
            this.DNSSeeds = new List<DNSSeedData>();

            this.SeedNodes = new List<NetworkAddress>();

            RegisterRules(this.Consensus);
        }

        private static void RegisterRules(IConsensus consensus)
        {
            consensus.ConsensusRules
                .Register<HeaderTimeChecksRule>()
                .Register<HeaderTimeChecksPosRule>()
                .Register<PosFutureDriftRule>()
                .Register<CheckDifficultyPosRule>()
                .Register<X1HeaderVersionRule>()
                .Register<ProvenHeaderSizeRule>()
                .Register<ProvenHeaderCoinstakeRule>()
                .Register<BlockMerkleRootRule>()
                .Register<PosBlockSignatureRepresentationRule>()
                .Register<PosBlockSignatureRule>()
                .Register<SetActivationDeploymentsPartialValidationRule>()
                .Register<PosTimeMaskRule>()
                .Register<X1RequireWitnessRule>()
                .Register<X1EmptyScriptSigRule>()
                .Register<TransactionLocktimeActivationRule>()
                .Register<CoinbaseHeightActivationRule>()
                .Register<WitnessCommitmentsRule>()
                .Register<BlockSizeRule>()
                .Register<EnsureCoinbaseRule>()
                .Register<CheckPowTransactionRule>()
                .Register<CheckPosTransactionRule>()
                .Register<CheckSigOpsRule>()
                .Register<PosCoinstakeRule>()
                .Register<X1PosPowRatchetRule>()
                .Register<SetActivationDeploymentsFullValidationRule>()
                .Register<CheckDifficultyHybridRule>()
#pragma warning disable CS0618 // Type or member is obsolete
                .Register<LoadCoinviewRule>()
#pragma warning restore CS0618 // Type or member is obsolete
                .Register<TransactionDuplicationActivationRule>()
                .Register<X1PosCoinviewRule>()
                .Register<PosColdStakingRule>()
#pragma warning disable CS0618 // Type or member is obsolete
                .Register<SaveCoinviewRule>();
#pragma warning restore CS0618 // Type or member is obsolete

            consensus.MempoolRules = new List<Type>
            {
                typeof(CheckConflictsMempoolRule),
                typeof(CheckCoinViewMempoolRule),
                typeof(CreateMempoolEntryMempoolRule),
                typeof(X1RequireWitnessMempoolRule),
                typeof(X1EmptyScriptSigMempoolRule),
                typeof(CheckSigOpsMempoolRule),
                typeof(X1CheckFeeMempoolRule),
                typeof(CheckRateLimitMempoolRule),
                typeof(CheckAncestorsMempoolRule),
                typeof(CheckReplacementMempoolRule),
                typeof(CheckAllInputsMempoolRule)
            };
        }
    }
}