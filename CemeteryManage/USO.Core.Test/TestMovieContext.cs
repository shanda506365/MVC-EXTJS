namespace USO.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using NUnit.Framework;

    public class MovieInitializer : DropCreateDatabaseAlways<MovieDBContext>
    {
        protected override void Seed(MovieDBContext context)
        {
            var movies = new List<Movie> { 
 
                 new Movie { Title = "When Harry Met Sally",  
                             ReleaseDate=DateTime.Parse("1989-1-11"),  
                             Genre="Romantic Comedy", 
                             Rating="R", 
                             Price=7.00M}, 
 
                 new Movie { Title = "Ghostbusters 2",  
                             ReleaseDate=DateTime.Parse("1986-2-23"),  
                             Genre="Comedy", 
                             Rating="R", 
                             Price=9.1111111111111M},  
             };

            movies.ForEach(d => context.Movies.Add(d));
        }
    }
    public class Movie
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string Rating { get; set; }
    }

    public class Actor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public Movie theMovie { get; set; }
        public int MovieID { get; set; }
    }
    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().Property(a => a.Price).HasPrecision(19, 5);
        }
    }

    [TestFixture]
    public class TestMovieContext
    {
        [Test]
        public void TestInitDB()
        {
            Database.SetInitializer<MovieDBContext>(new MovieInitializer());
            var db = new MovieDBContext();
            Assert.AreEqual(2, db.Movies.Count());
        }

        [Test]
        public void TestInsertMovies()
        {
            var dbContext = new MovieDBContext();
            var movie =  new Movie { Title = "大话西游",  
                             ReleaseDate=DateTime.Parse("1994-1-1"),  
                             Genre="Comedy", 
                             Rating="R", 
                             Price=99.999M};
            dbContext.Movies.Add(movie);
            dbContext.SaveChanges();
            Assert.AreEqual("大话西游", dbContext.Movies.OrderByDescending(a => a.ID).Select(a => a.Title).FirstOrDefault());

        }

        [Test]
        public void TestInsertActors()
        {
            var dbContext = new MovieDBContext();
            var actor = new Actor
                            {
                                Name = "XXX",
                                theMovie = new Movie
                                               {
                                                   Title = "大话西游2",
                                                   ReleaseDate = DateTime.Parse("2013-1-1"),
                                                   Genre = "Comedy",
                                                   Rating = "R",
                                                   Price = 999.999M
                                               },
                            };
            dbContext.Actors.Add(actor);
            dbContext.SaveChanges();
        }

    }
}
