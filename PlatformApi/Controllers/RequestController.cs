using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Http;
using PlatformApi.Models;

namespace PlatformApi.Controllers {
public class RequestController : ApiController
{
    private string connectionString = WebConfigurationManager.ConnectionStrings["DbPlatformConnection"].ConnectionString;

    // GET: api/Request
    public IHttpActionResult Get()
    {
        List<Request> requests = new List<Request>();
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var command = new SqlCommand("SELECT * FROM Requests", conn);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Request request = new Request
                {
                    RequestId = (int)reader["RequestId"],
                    Title = (string)reader["Title"],
                    Description = (string)reader["Description"],
                    CreationDate = (DateTime)reader["CreationDate"]
                };
                requests.Add(request);
            }
        }
        return Ok(requests);
    }

    // GET: api/Request/5
    public IHttpActionResult Get(int id)
    {
        Request request = null;
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var command = new SqlCommand("SELECT * FROM Requests WHERE RequestId = @RequestId", conn);
            command.Parameters.AddWithValue("@RequestId", id);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                request = new Request
                {
                    RequestId = (int)reader["RequestId"],
                    Title = (string)reader["Title"],
                    Description = (string)reader["Description"],
                    CreationDate = (DateTime)reader["CreationDate"]
                };
            }
        }
        if (request == null)
        {
            return NotFound();
        }
        return Ok(request);
    }

    // POST: api/Request
    public IHttpActionResult Post(Request request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var command = new SqlCommand(@"
                INSERT INTO Requests (Title, Description, CreationDate)
                VALUES (@Title, @Description, @CreationDate)
                SELECT SCOPE_IDENTITY()
            ", conn);
            command.Parameters.AddWithValue("@Title", request.Title);
            command.Parameters.AddWithValue("@Description", request.Description);
            command.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            int newRequestId = Convert.ToInt32(command.ExecuteScalar());
            request.RequestId = newRequestId;
        }
        return CreatedAtRoute("DefaultApi", new { id = request.RequestId }, request);
    }

    // PUT: api/Request/5
    public IHttpActionResult Put(int id, Request request)
    {
        if (!ModelState.IsValid || id != request.RequestId)
        {
            return BadRequest(ModelState);
        }
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var command = new SqlCommand(@"
                UPDATE Requests
                SET Title = @Title, Description = @Description
                WHERE RequestId = @RequestId
            ", conn);
            command.Parameters.AddWithValue("@Title", request.Title);
            command.Parameters.AddWithValue("@Description", request.Description);
            command.Parameters.AddWithValue("@RequestId", id);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                return NotFound();
            }
        }
        return Ok(request);
    }

    // DELETE: api/Request/5
    public IHttpActionResult Delete(int id)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            conn.Open();

            var command = new SqlCommand("DELETE FROM Requests WHERE RequestId = @RequestId", conn);
            command.Parameters.AddWithValue("@RequestId", id);
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

