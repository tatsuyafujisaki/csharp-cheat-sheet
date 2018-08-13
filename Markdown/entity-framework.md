# Example

```csharp
static void Main()
{
    using (var dc = new DbContext1())
    {
        dc.Database.Log = s => Debug.Print(s);

        // Do something with dc
    }
}

static IEnumerable<Table1> SelectAll(DbContext1 dc) => dc.Table1;

static Table1 SelectByPrimaryKey(DbContext1 dc, params object[] primaryKeyValues) => dc.Table1.Find(primaryKeyValues);

static void SelectNotByPrimaryKey(DbContext1 dc, Expression<Func<Table1, bool>> predicate) => dc.Table1.SingleOrDefault(predicate);

static void Insert(DbContext1 dc, Table1 record)
{
    dc.Table1.Add(record);
    dc.SaveChange();
}

static void Delete(DbContext1 dc, Table1 record)
{
    if (record == null)
    {
      return;
    }

    dc.Entry(record).State = EntityState.Deleted;
    dc.SaveChanges();
}

static void UpdateColumn1(DbContext1 dc, Table1 record, string s)
{
    if (record == null)
    {
      return;
    }

    record.Column1 = s;
    dc.SaveChanges();
}
```

# How to troubleshoot an error occurring when you try to insert/update/delete a record using Entity Framework 6
1. Add a primary key to the table.
2. Delete and create the table in the Entity Framework designer because the designer does not automatically get updated.

# References
## Connection string syntax
* https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/connection-string-syntax
* https://docs.microsoft.com/en-us/sql/ado/guide/appendixes/microsoft-ole-db-provider-for-sql-server
* https://docs.microsoft.com/en-us/sql/relational-databases/native-client/applications/using-ado-with-sql-server-native-client