using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Http;
using PlatformApi.Models;

namespace PlatformApi.Controllers
{
    public class ResponseController : ApiController
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["DbPlatformConnection"].ConnectionString;

        // GET: api/Response
        public IHttpActionResult Get()
        {
            List<Response> responses = new List<Response>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Responses", conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Response response = new Response
                    {
                        ResponseId = (int)reader["ResponseId"],
                        Content = (string)reader["Content"],
                        UploadDate = (DateTime)reader["UploadDate"]
                    };
                    responses.Add(response);
                }
            }
            return Ok(responses);
        }

        // GET: api/Response/5
        public IHttpActionResult Get(int id)
        {
            Response response = null;
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Responses WHERE ResponseId = @ResponseId", conn);
                command.Parameters.AddWithValue("@ResponseId", id);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    response = new Response
                    {
                        ResponseId = (int)reader["ResponseId"],
                        Content = (string)reader["Content"],
                        UploadDate = (DateTime)reader["UploadDate"]
                    };
                }
            }
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // POST: api/Response
        public IHttpActionResult Post(Response response)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand(@"
                INSERT INTO Responses (Content, UploadDate)
                VALUES (@Content, @UploadDate)
                SELECT SCOPE_IDENTITY()
            ", conn);
                command.Parameters.AddWithValue("@Content", response.Content);
                command.Parameters.AddWithValue("@UploadDate", DateTime.Now);
                int newResponseId = Convert.ToInt32(command.ExecuteScalar());
                response.ResponseId = newResponseId;
            }
            return CreatedAtRoute("DefaultApi", new { id = response.ResponseId }, response);
        }

        // PUT: api/Response/5
        public IHttpActionResult Put(int id, Response response)
        {
            if (!ModelState.IsValid || id != response.ResponseId)
            {
                return BadRequest(ModelState);
            }
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand(@"
                UPDATE Responses
                SET Content = @Content
                WHERE ResponseId = @ResponseId
            ", conn);
                command.Parameters.AddWithValue("@Content", response.Content);
                command.Parameters.AddWithValue("@ResponseId", id);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }
            return Ok(response);
        }

        // DELETE: api/Response/5
        public IHttpActionResult Delete(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("DELETE FROM Responses WHERE ResponseId = @ResponseId", conn);
                command.Parameters.AddWithValue("@ResponseId", id);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }
            return Ok();
        }
    }
}
