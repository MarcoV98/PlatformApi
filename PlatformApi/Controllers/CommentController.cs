using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Http;
using PlatformApi.Models;

namespace PlatformApi.Controllers
{
    public class CommentController : ApiController
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["DbPlatformConnection"].ConnectionString;

        // GET: api/Comment
        public IHttpActionResult Get()
        {
            List<Comment> comments = new List<Comment>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Comments", conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Comment comment = new Comment
                    {
                        CommentId = (int)reader["CommentId"],
                        Content = (string)reader["Content"],
                        CommentDate = (DateTime)reader["CommentDate"]
                    };
                    comments.Add(comment);
                }
            }
            return Ok(comments);
        }

        // GET: api/Comment/5
        public IHttpActionResult Get(int id)
        {
            Comment comment = null;
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Comments WHERE CommentId = @CommentId", conn);
                command.Parameters.AddWithValue("@CommentId", id);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    comment = new Comment
                    {
                        CommentId = (int)reader["CommentId"],
                        Content = (string)reader["Content"],
                        CommentDate = (DateTime)reader["CommentDate"]
                    };
                }
            }
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // POST: api/Comment
        public IHttpActionResult Post(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand(@"
                INSERT INTO Comments (Content, CommentDate)
                VALUES (@Content, @CommentDate)
                SELECT SCOPE_IDENTITY()
            ", conn);
                command.Parameters.AddWithValue("@Content", comment.Content);
                command.Parameters.AddWithValue("@CommentDate", DateTime.Now);
                int newCommentId = Convert.ToInt32(command.ExecuteScalar());
                comment.CommentId = newCommentId;
            }
            return CreatedAtRoute("DefaultApi", new { id = comment.CommentId }, comment);
        }

        // PUT: api/Comment/5
        public IHttpActionResult Put(int id, Comment comment)
        {
            if (!ModelState.IsValid || id != comment.CommentId)
            {
                return BadRequest(ModelState);
            }
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand(@"
                UPDATE Comments
                SET Content = @Content
                WHERE CommentId = @CommentId
            ", conn);
                command.Parameters.AddWithValue("@Content", comment.Content);
                command.Parameters.AddWithValue("@CommentId", id);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }
            return Ok(comment);
        }

        // DELETE: api/Comment/5
        public IHttpActionResult Delete(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("DELETE FROM Comments WHERE CommentId = @CommentId", conn);
                command.Parameters.AddWithValue("@CommentId", id);
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
