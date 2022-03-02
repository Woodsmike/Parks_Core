using Microsoft.EntityFrameworkCore;
using ParksAPI.Data;
using ParksAPI.Models;
using ParksAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParksAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int TrailId)
        {
            return _db.Trails.Include(a => a.NationalPark).FirstOrDefault(i => i.Id == TrailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(a => a.NationalPark).OrderBy(a => a.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(a => a.NationalPark).Where(a => a.Id == npId).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = _db.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(a => a.Id == id);           
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }
    }
}
