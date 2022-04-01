using System;
using System.Reflection;
using System.ComponentModel;
public class DbEntity<T> where T : class, new()
{
    // private string _dbType;
    // private string _sqlParamIndexChar;
    // private string _insertedIdSql;
    private IDbOption _dbOption;
    private T _source;
    private string _tableName;
    private string _key;
    private List<string> _fields;
    private List<string> _fieldsWithoutKeyAndNoedit;
    private List<string> _filedsWithOnlyEdit;
    public DbEntity(IDbOption dbOption)
    {
        // if (dbOption == null){
        //     this._dbOption = new DbOption()
        //     {
        //         dbType = "ms-sql",
        //         connectionString = "Data Source=localhost;Initial Catalog=testdb;Integrated Security=True;MultipleActiveResultSets=True",
        //         sqlParamIndexChar = "@",
        //         insertedIdSql = "select SCOPE_IDENTITY();"
        //     };
        // }
        this._dbOption = dbOption;
        this._source = new T();
        this._tableName = this.GetTableName();
        this._key = this.GetKey();
        this._fields = this.GetFields();
        this._fieldsWithoutKeyAndNoedit = this._fields.Except(this.GetFieldsWithNoEdit().Append(this._key)).ToList();
        this._filedsWithOnlyEdit = this._fields.Except(this.GetFieldsWithNoEdit()).ToList();
    }

    public DbEntity() : this(new DbOption()
            {
                dbType = "ms-sql",
                connectionString = "Data Source=localhost;Initial Catalog=testdb;Integrated Security=True;MultipleActiveResultSets=True",
                sqlParamIndexChar = "@",
                insertedIdSql = "select SCOPE_IDENTITY();"
            }){}

    public string SqlInsert(bool blnRange = false){
        string sql = $@"insert into {this._tableName}
            ({string.Join(", ", this._fieldsWithoutKeyAndNoedit)}) 
            values({string.Join(", ", this._fieldsWithoutKeyAndNoedit.Select(s=> $"{this._dbOption.sqlParamIndexChar}{s}"))})  
        ";
        if (blnRange == false)
        {
            sql += this._dbOption.insertedIdSql;
/*             switch(this._dbType.ToLower())
            {
                case "ms-sql":
                    sql += "select SCOPE_IDENTITY();";
                    break;
                case "postgres":
                    sql += sql.Replace(";", "") +" RETURNING id;";
                    break;
                default: 
                    sql += "select SCOPE_IDENTITY();";
                    break;
            }
 */
         }
        return sql;
    }

    public string SqlSelect(){
        string sql = $@"select * from {this._tableName} ";
        return sql;
    }

    public string SqlSelectById(){
        string sql = $@"select * from {this._tableName} 
            where {this._key} = {this._dbOption.sqlParamIndexChar}{this._key} ";
        return sql;
    }

    public string SqlUpdateById(){
        string sql = $@"update {this._tableName}
            set {String.Join(", ", this._fieldsWithoutKeyAndNoedit.Select(s=> $"{s} = {this._dbOption.sqlParamIndexChar}{s}"))}
            where {this._key} = {this._dbOption.sqlParamIndexChar}{this._key} 
        ";
        return sql;
    }

    public string SqlDeleteById(){
        string sql = $@"delete from {this._tableName} 
            where {this._key} = {this._dbOption.sqlParamIndexChar}{this._key} ";
        return sql;
    }

    public string SqlTruncate(){
        string sql = $@"truncate table {this._tableName} ";
        return sql;
    }

    public void ListTable(){
        // Console.WriteLine(this._tableName);
        // Console.WriteLine(this._key);
        // Console.WriteLine(String.Join(", ", this._fields));
        // Console.WriteLine(String.Join(", ", this._filedsWithOnlyEdit));
        Console.WriteLine(this.SqlInsert());
        Console.WriteLine(this.SqlSelect());
        Console.WriteLine(this.SqlSelectById());
        Console.WriteLine(this.SqlUpdateById());
        Console.WriteLine(this.SqlDeleteById());
        Console.WriteLine(this.SqlTruncate());
        // Console.WriteLine(this.GetTableName());
        // Console.WriteLine(this.key());
        // Console.WriteLine(String.Join(", ", this.fields()));
        // Console.WriteLine("!");
        // Console.WriteLine(String.Join(", ", this.edit()));
        // Console.WriteLine("!");
        return ;

/*         var key = GetType().GetProperties().FirstOrDefault(gt => gt.GetCustomAttributes().Any(attr => attr.GetType().Name == "KeyAttribute"));
        Console.WriteLine(String.Join(", ", key.Name));
        var fields = GetType().GetProperties().Where(gt => gt.GetCustomAttributes().Count() == 0).Select(s=>s.Name).ToList();
        Console.WriteLine(String.Join(", ", fields));
        var props = GetType().GetProperties().Where(d => d.Name != key.Name);
        Console.WriteLine(String.Join(", ", props.Select(s=>s.Name)));

        //Attribute.GetCustomAttributes(this)
        dynamic attributeTable = this.GetType().GetCustomAttributes(false)
            .SingleOrDefault(attr => attr.GetType().Name == "TableAttribute");

        return;// attributeTable != null ? attributeTable.Name : this.GetType().Name;
 */    }

    private string GetTableName(){
        dynamic? attributeTable = this._source.GetType().GetCustomAttributes(false)
            .SingleOrDefault(attr => attr.GetType().Name == "TableAttribute");
        return attributeTable != null ? attributeTable.Name : this.GetType().Name;
    }

    private string GetKey(){
        var key = this._source.GetType().GetProperties().FirstOrDefault(gt => gt.GetCustomAttributes()
            .Any(attr => attr.GetType().Name == "KeyAttribute"));
        return key?.Name ?? "id";
    }

    private List<string> GetFields(){
        var fields = this._source.GetType().GetProperties().Select(s=>s.Name).ToList();
        return fields;
    }
    private IEnumerable<string> GetFieldsWithNoEdit(){
        var edit = this._source.GetType().GetProperties().Where(gt => gt.GetCustomAttributes()
            .Any(attr => attr.GetType().Name == "EditableAttribute" 
            && (attr as System.ComponentModel.DataAnnotations.EditableAttribute)?.AllowEdit == false
        )).Select(s=>s.Name);//;.ToList();
        return edit;// new List<string>();
    }

}