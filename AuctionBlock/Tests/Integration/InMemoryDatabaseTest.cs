using System;
using System.Data;
using AuctionBlock.DataAccess.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.SqlTypes;
using NHibernate.Tool.hbm2ddl;
using NHibernate.UserTypes;
using NUnit.Framework;
using StructureMap;
using Environment = NHibernate.Cfg.Environment;

namespace AuctionBlock.Tests.Integration
{
    [TestFixture]
    public abstract class InMemoryDatabaseTest : IDisposable
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        protected ISession Session;

        [SetUp]
        public void SetupBase()
        {
            Session = _sessionFactory.OpenSession();
            new SchemaExport(_configuration)
                .Execute(true, true, false, Session.Connection, Console.Out);
            ObjectFactory.Initialize(x => x.For<ISession>().Add(Session));
            SetUp();
        }

        protected virtual void SetUp()
        {}

        public InMemoryDatabaseTest()
        {
            SetProcessorTypeForSqliteHack();

            if (_configuration == null)
            {
                _configuration =
                    Fluently.Configure()
                            .Database(
                                SQLiteConfiguration
                                    .Standard
                                    .ConnectionString("data source=:memory:"))
                            .Mappings(
                                m => m.FluentMappings
                                    .AddFromAssemblyOf<AuctionMap>()
                                    .Conventions
                                        .Add(
                                            new NormalizedDateTimeUserTypeConvention(), 
                                            new NormalizedNullableDateTimeUserTypeConvention()))
                            .ExposeConfiguration(
                                c =>
                                    {
                                        c.Properties.Add(Environment.ReleaseConnections, "on_close");
                                        new SchemaExport(c).Create(false, true);
                                    })
                            .BuildConfiguration();

                _sessionFactory = _configuration.BuildSessionFactory();
            }

            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();            
        }

        protected void TransactionScope(Action transactionLogic)
        {
            using (var transaction = Session.BeginTransaction())
            {
                transactionLogic.Invoke();
                    
                transaction.Commit();
            }
        }

        public void Dispose()
        {
            Session.Dispose();
        }

        // Needed to be able to sucessfully load the SQLite interop DLL
        // See for details: http://stackoverflow.com/questions/13028069/unable-to-load-dll-sqlite-interop-dll
        private void SetProcessorTypeForSqliteHack()
        {
            int wsize = IntPtr.Size;
            string libdir = (wsize == 4) ? "x86" : "x64";
            string appPath = System.IO.Path.GetDirectoryName(System.Environment.CurrentDirectory);
            SetDllDirectory(System.IO.Path.Combine(appPath, libdir));
        }

        // Needed to be able to sucessfully load the SQLite interop DLL
        // See for details: http://stackoverflow.com/questions/13028069/unable-to-load-dll-sqlite-interop-dll
        [System.Runtime.InteropServices.DllImport("kernel32.dll",
            CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetDllDirectory(string lpPathName);

        public class NormalizedDateTimeUserType : IUserType
        {
            private readonly TimeZoneInfo databaseTimeZone = TimeZoneInfo.Local;


            public virtual Type ReturnedType
            {
                get { return typeof(DateTimeOffset); }
            }

            public virtual bool IsMutable
            {
                get { return false; }
            }

            public virtual object Disassemble(object value)
            {
                return value;
            }

            public virtual SqlType[] SqlTypes
            {
                get { return new[] { new SqlType(DbType.DateTime) }; }
            }

            public virtual bool Equals(object x, object y)
            {
                return object.Equals(x, y);
            }

            public virtual int GetHashCode(object x)
            {
                return x.GetHashCode();
            }

            public virtual object NullSafeGet(IDataReader dr, string[] names,
                                              object owner)
            {
                object r = dr[names[0]];
                if (r == DBNull.Value)
                {
                    return null;
                }

                DateTime storedTime = (DateTime)r;
                return new DateTimeOffset(storedTime,
                                          this.databaseTimeZone.BaseUtcOffset);
            }

            public virtual void NullSafeSet(IDbCommand cmd, object value, int index)
            {
                if (value == null)
                {
                    NHibernateUtil.DateTime.NullSafeSet(cmd, null, index);
                }
                else
                {
                    IDataParameter parameter = (IDataParameter)cmd.Parameters[index];
                    try
                    {
                        DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
                        DateTime paramVal =
                            dateTimeOffset.ToOffset(this.databaseTimeZone.BaseUtcOffset).DateTime;

                        parameter.Value = paramVal;
                    }
                    catch (Exception e)
                    {
                        parameter.Value = DateTime.MinValue;
                    }
                }
            }

            public virtual object DeepCopy(object value)
            {
                return value;
            }

            public virtual object Replace(object original, object target, object
                                                                              owner)
            {
                return original;
            }

            public virtual object Assemble(object cached, object owner)
            {
                return cached;
            }
        }

        public class NormalizedNullabeDateTimeUserType :
            NormalizedDateTimeUserType
        {
            public override Type ReturnedType
            {
                get { return typeof(DateTimeOffset?); }
            }
        }

        public class NormalizedDateTimeUserTypeConvention :
            UserTypeConvention<NormalizedDateTimeUserType>
        {

        }

        public class NormalizedNullableDateTimeUserTypeConvention :
            UserTypeConvention<NormalizedNullabeDateTimeUserType>
        {

        }
    }
}