using Microsoft.EntityFrameworkCore;
using Robo.Database.Models;

namespace Robo.Database.Context
{
    public partial class TickerContext : DbContext
    {
        public TickerContext()
        {
        }

        public TickerContext(DbContextOptions<TickerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Candles> Candles { get; set; }
        public virtual DbSet<CurExchIdentifers> CurExchIdentifers { get; set; }
        public virtual DbSet<CurPairIdentifiers> CurPairIdentifiers { get; set; }
        public virtual DbSet<Exchanges> Exchanges { get; set; }
        public virtual DbSet<ExchangesCurPairs> ExchangesCurPairs { get; set; }
        public virtual DbSet<Extremums> Extremums { get; set; }
        public virtual DbSet<ExtremumsLines> ExtremumsLines { get; set; }
        public virtual DbSet<InvIndex> InvIndex { get; set; }
        public virtual DbSet<InvMa> InvMa { get; set; }
        public virtual DbSet<Ledger> Ledger { get; set; }
        public virtual DbSet<OperationalInformationOfTsk> OperationalInformationOfTsk { get; set; }
        public virtual DbSet<PlxIdPairs> PlxIdPairs { get; set; }
        public virtual DbSet<PlxSummary> PlxSummary { get; set; }
        public virtual DbSet<TaskScan> TaskScan { get; set; }
        public virtual DbSet<TaskStrategyCheckDeep> TaskStrategyCheckDeep { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<TasksHistories> TasksHistories { get; set; }
        public virtual DbSet<TasksLedger> TasksLedger { get; set; }
        public virtual DbSet<TasksListnerStates> TasksListnerStates { get; set; }
        public virtual DbSet<TasksLists> TasksLists { get; set; }
        public virtual DbSet<TasksStats> TasksStats { get; set; }
        public virtual DbSet<Ticker> Ticker { get; set; }
        public virtual DbSet<TrueDiapasons> TrueDiapasons { get; set; }
        public virtual DbSet<Tt> Tt { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VolatilityRatio> VolatilityRatio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candles>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Close).HasComment("last execution during the time frame");

                entity.Property(e => e.High).HasComment("highest execution during the time frame");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Low).HasComment("lowest execution during the time frame");

                entity.Property(e => e.Mtc).HasComment("mtc parameter which determine timeframe border - time of start and time of end");

                entity.Property(e => e.Open).HasComment("first execution during the time frame");

                entity.Property(e => e.ReceiptTime).HasComment("time of receipt tickers data in");

                entity.Property(e => e.SlicePeriod).HasComment("parameter which determines time period between request or updates data from exchange");

                entity.Property(e => e.TimeFrame).HasComment("timeframe parameter - 1m, 5m, 1h and etc");

                entity.Property(e => e.Volume).HasComment("quantity of symbol traded within the timeframe");
            });

            modelBuilder.Entity<CurExchIdentifers>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<CurPairIdentifiers>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Exchanges>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Code).HasComment("shortname of exchange");

                entity.Property(e => e.Name).HasComment("fullname of exchange");

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("1")
                    .HasComment("1 for active, 0 for inactive, default is 1");
            });

            modelBuilder.Entity<ExchangesCurPairs>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ExtId).HasComment("internal id of currency pair on exchange");

                entity.Property(e => e.Id).HasDefaultValueSql("nextval('exchanges_cur_pairs_id'::regclass)");

                entity.Property(e => e.Name).HasComment("name of currency pair");

                entity.Property(e => e.Status).HasComment("1 for active, 0 for inactive");
            });

            modelBuilder.Entity<Extremums>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ExtremumType).HasComment("abs_min, abs_max, rel_min, rel_max");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ExtremumsLines>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Counter).HasComment("if counter = 3 then new record must be inserted");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LineName).HasComment("e.g. positive by absMax point");

                entity.Property(e => e.Trend).HasComment("p - positive, n - negative");
            });

            modelBuilder.Entity<InvIndex>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<InvMa>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Ledger>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CondForOp).HasComment(@"показатели для проведения операции
бывашая inv_ma");

                entity.Property(e => e.Condition).HasComment("бывшая inv_index, теперь тут условия для входа");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<OperationalInformationOfTsk>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<PlxIdPairs>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<PlxSummary>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TaskScan>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<TaskStrategyCheckDeep>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(e => e.Id).HasComment("pk");

                entity.Property(e => e.IsActive).HasComment("true = if task is working or start");
            });

            modelBuilder.Entity<TasksHistories>(entity =>
            {
                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TasksHistories)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tasks_id_fkey");
            });

            modelBuilder.Entity<TasksLedger>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<TasksListnerStates>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"TasksListenStates_Id_seq\"'::regclass)");
            });

            modelBuilder.Entity<TasksLists>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<TasksStats>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Ticker>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LastPrice).HasComment("price of the last trade");

                entity.Property(e => e.ReceiptTime).HasComment("time of receipt tickers data in");
            });

            modelBuilder.Entity<TrueDiapasons>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CurMaxTime).HasComment("время максимума");

                entity.Property(e => e.CurMaxValue).HasComment("максимум n-часового периода");

                entity.Property(e => e.CurMinTime).HasComment("время минимума");

                entity.Property(e => e.CurMinValue).HasComment("минимум n-часового периода");

                entity.Property(e => e.CurrencyId).HasComment("валютная пара");

                entity.Property(e => e.DateShot).HasComment("время слепка");

                entity.Property(e => e.NPeriod).HasComment("значение периода");

                entity.Property(e => e.PrevCloseTime).HasComment("время цены закрытия");

                entity.Property(e => e.PrevCloseValue).HasComment("цена закрытия n-часового периода");

                entity.Property(e => e.TrueDiapason).HasComment("истинный диапазон");

                entity.Property(e => e.TrueMax).HasComment("истинный максимум");

                entity.Property(e => e.TrueMin).HasComment("истинный минимум");
            });

            modelBuilder.Entity<Tt>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VolatilityRatio>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CurrencyId).HasComment("валютная пара");

                entity.Property(e => e.DateShot).HasComment("время слепка");

                entity.Property(e => e.Period).HasComment("период для расчета");

                entity.Property(e => e.Vr).HasComment("коэффициент волатильности");
            });

            modelBuilder.HasSequence("exchanges_cur_pairs_id");

            modelBuilder.HasSequence("exchanges_id_seq");

            modelBuilder.HasSequence("extremums_id_seq");

            modelBuilder.HasSequence("extremums_lines_id_seq");

            modelBuilder.HasSequence("tasks_histories_Id_seq");

            modelBuilder.HasSequence("TasksListenStates_Id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
