using System.Linq;

namespace DoAn_LapTrinhWeb
{
    public class CheckSlug
    {
        private readonly DbContext db = new DbContext();

        public bool KiemTraSlug(string Table, string Slug, int? id)
        {
            switch (Table)
            {
                case "Category":
                    if (id != null)
                    {
                        if (db.Genres.Where(m => m.slug == Slug && m.genre_id != id).Count() > 0)
                            return false;
                    }
                    else
                    {
                        if (db.Genres.Where(m => m.slug == Slug).Count() > 0)
                            return false;
                    }

                    break;
                case "Topic":
                    break;
                case "Post":
                    break;
                case "Product":
                    break;
            }

            return true;
        }
    }
}