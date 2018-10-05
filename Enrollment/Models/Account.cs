using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Enrollment.Models
{
    public class Account
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter a valid username.")]
        public string Username { get; set; }

        //public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a valid password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public interface IAccountRepository
    {
        List<Account> GetAll();
        Account Find(int id);
        Account Add(Account account);
        Account Update(Account account);
        void Remove(int id);
        Account GetAccountInformatiom(int id);
        Account Validate(Account account);
    }

    public class AccountRepository : IAccountRepository
    {
        private IDbConnection _db = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        public List<Account> GetAll()
        {
            return this._db.Query<Account>("SELECT * FROM \"Accounts\"").ToList();
        }

        public Account Find(int id)
        {
            return this._db.Query<Account>
            ("SELECT * FROM \"Accounts\" WHERE id = @AccountID", new { id }).SingleOrDefault();
        }

        public Account Add(Account account)
        {
            var sqlQuery = "INSERT INTO \"Accounts\"" +
                "(username, password) VALUES(@Username, @Password); " + 
                "RETURNING id";
            var accountId = this._db.Query<int>(sqlQuery, account).Single();
            account.ID = accountId;
            return account;
        }

        public Account Update(Account account)
        {
            var sqlQuery =
                "UPDATE \"Accounts\" " +
                "SET username = @Username, " +
                //"    Email     = @Email " +
                "password = @Password" +
                "WHERE id = @AccountID";
            this._db.Execute(sqlQuery, account);
            return account;
        }

        public void Remove(int id)
        {
            var sqlQuery = "delete from \"Accounts\" where id = @ID";
            this._db.Execute(sqlQuery, new { id });
        }

        public Account GetAccountInformatiom(int id)
        {
            throw new NotImplementedException();
        }

        public Account Validate(Account account)
        {
            var sqlQuery = "select * from \"Accounts\" where username = '"+ account.Username +"' and password = '"+ account.Password +"'";

            //var sqlQuery = "select id, username from \"Accounts\" where username = '" + account.Username + "' and password = '" + account.Password + "'";
            var anotheraccount = this._db.QueryFirstOrDefault<Account>(sqlQuery);
            return anotheraccount;
        }
    }
}