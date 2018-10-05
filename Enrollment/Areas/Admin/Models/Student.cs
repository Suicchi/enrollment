using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Enrollment.Areas.Admin.Models
{
    public class Student
    {

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage ="Name cannot be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Father's Name cannot be empty")]
        public string FathersName { get; set; }

        [Required(ErrorMessage = "Mother's Name cannot be empty")]
        public string MothersName { get; set; }

        [Required(ErrorMessage = "cannot be empty")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Present Address cannot be empty")]
        public string PresentAddress { get; set; }

        [Required(ErrorMessage = "Permanent Address cannot be empty")]
        public string PermanentAddress { get; set; }

        [Required(ErrorMessage = "Date of birth cannot be empty")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }

        [Required(ErrorMessage = "Religion cannot be empty")]
        public string Religion { get; set; }

        [Required(ErrorMessage = "Phone number cannot be empty")]
        public string Phone { get; set; }

        //[Required(ErrorMessage = " can be empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "AdmissionDate cannot be empty")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AdmissionDate { get; set; }
    }

    public interface IStudentRepository
    {
        List<Student> GetAll();
        List<Student> FindAsList(int id);
        List<Student> FindByName(string name);
        Student Find(int id);
        Student Add(Student student);
        Student Update(Student student);
        void Remove(int id);
        //Student GetStudentInformatiom(int id);
        //Student Validate(Student student);
    }

    public class StudentRepository : IStudentRepository
    {
        private IDbConnection _db = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        public List<Student> GetAll()
        {
            return this._db.Query<Student>("SELECT * FROM \"Students\"").ToList();
        }

        public Student Add(Student student)
        {
            var sqlQuery = "INSERT INTO \"Students\"" +
                "(name, fathersname, mothersname, gender, \"presentaddress\", \"permanentaddress\", \"dateofbirth\", \"religion\", \"phone\", \"email\", \"admissiondate\" ) " +
                "VALUES(@Name, @FathersName, @MothersName, @Gender, @PresentAddress, @PermanentAddress, @DateofBirth, @Religion, @Phone, @Email, @AdmissionDate) " +
                "RETURNING id";
            var accountId = this._db.Query<int>(sqlQuery, student).Single();
            student.ID = accountId;
            return student;
        }

        public List<Student> FindAsList(int id)
        {
            return this._db.Query<Student>
            ("SELECT * FROM \"Students\" WHERE id = @ID", new { id }).ToList();
        }

        public Student Find(int id)
        {
            return this._db.Query<Student>
            ("SELECT * FROM \"Students\" WHERE id = @ID", new { id }).SingleOrDefault();
        }

        public List<Student> FindByName(string name)
        {
            return this._db.Query<Student>
            ($"SELECT * FROM \"Students\" WHERE name like @Name ", new { name }).ToList();
        }

        //public Student GetStudentInformatiom(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public void Remove(int id)
        {
            var sqlQuery = "delete from \"Students\" where id=@ID";
            this._db.Execute(sqlQuery, new { id });
        }

        public Student Update(Student student)
        {
            var sqlQuery =
                "UPDATE \"Students\" " +
                "SET name = @Name, " +
                "fathersname = @FathersName, " +
                "mothersname = @MothersName, " +
                "gender = @Gender, " +
                "presentaddress = @PresentAddress, " +
                "permanentaddress = @PermanentAddress, " +
                "dateofbirth = @DateOfBirth, " +
                "religion = @Religion," +
                "phone = @Phone, " +
                "email = @Email, " +
                "admissiondate = @AdmissionDate " +
                "WHERE id = @ID";
            this._db.Execute(sqlQuery, student);
            return student;
        }

    }
}