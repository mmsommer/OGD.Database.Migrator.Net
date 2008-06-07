using System.Data;
using Migrator.Framework;
using NUnit.Framework;

namespace Migrator.Tests.Providers
{
    /// <summary>
    /// Base class for Provider tests for all tests including constraint oriented tests.
    /// </summary>
    public class TransformationProviderConstraintBase : TransformationProviderBase
    {

          [Test]
          public void AddPrimaryKey()
          {
              AddTable();
              _provider.AddPrimaryKey("PK_Test", "Test", "Id");
          }

          [Test]
          public void AddIndexedColumn()
          {
              AddTable();
              _provider.AddColumn("TestTwo", "Test", DbType.String, 50, ColumnProperty.Indexed);
          }

          [Test]
          public void AddUniqueColumn()
          {
              AddTable();
              _provider.AddColumn("TestTwo", "Test", DbType.String, 50, ColumnProperty.Unique);
          }

          [Test]
          public void AddForeignKey()
          {
              AddTableWithPrimaryKey();
              _provider.AddForeignKey("FK_Test_TestTwo", "TestTwo", "TestId", "Test", "Id");
              Assert.IsTrue( _provider.ConstraintExists( "TestTwo", "FK_Test_TestTwo" ) );
          }
          
          [Test]
          public void AddUniqueConstraint()
          {
              AddTableWithPrimaryKey();
              _provider.AddUniqueConstraint("UN_Test_TestTwo", "TestTwo", "TestId");
              Assert.IsTrue( _provider.ConstraintExists( "TestTwo", "UN_Test_TestTwo" ) );
          }
          
          [Test]
          public virtual void AddCheckConstraint()
          {
              AddTableWithPrimaryKey();
              _provider.AddCheckConstraint("CK_TestTwo_TestId", "TestTwo", "TestId>5");
              Assert.IsTrue( _provider.ConstraintExists( "TestTwo", "CK_TestTwo_TestId" ) );
          }

          [Test]
          public void RemoveForeignKey()
          {
              AddForeignKey();
              _provider.RemoveForeignKey("TestTwo", "FK_Test_TestTwo");
              Assert.IsFalse( _provider.ConstraintExists( "TestTwo", "FK_Test_TestTwo" ) );
          }
          
          [Test]
          public void RemoveUniqueConstraint()
          {
              AddUniqueConstraint();
              _provider.RemoveConstraint("TestTwo", "UN_Test_TestTwo");
              Assert.IsFalse( _provider.ConstraintExists("TestTwo", "UN_Test_TestTwo"));
          }
          
          [Test]
          public virtual void RemoveCheckConstraint()
          {
              AddCheckConstraint();
              _provider.RemoveConstraint("TestTwo", "CK_TestTwo_TestId");
              Assert.IsFalse( _provider.ConstraintExists( "TestTwo", "CK_TestTwo_TestId" ) );
          }

          [Test]
          public void RemoveUnexistingForeignKey()
          {
              AddForeignKey();
              _provider.RemoveForeignKey("abc", "FK_Test_TestTwo");
              _provider.RemoveForeignKey("abc", "abc");
              _provider.RemoveForeignKey("Test", "abc");
          }

          [Test]
          public void ConstraintExist()
          {
              AddForeignKey();
              Assert.IsTrue(_provider.ConstraintExists("TestTwo", "FK_Test_TestTwo"));
              Assert.IsFalse(_provider.ConstraintExists("abc", "abc"));
          }
          
          
          [Test]
          public void AddTableWithCompoundPrimaryKey()
          {
              _provider.AddTable("Test",
                                 new Column("PersonId", DbType.Int32, ColumnProperty.PrimaryKey),
                                 new Column("AddressId", DbType.Int32, ColumnProperty.PrimaryKey)
                  );
              Assert.IsTrue(_provider.TableExists("Test"), "Table doesn't exist");
              Assert.IsTrue(_provider.PrimaryKeyExists("Test", "PK_Test"), "Constraint doesn't exist");
          }
    }
}