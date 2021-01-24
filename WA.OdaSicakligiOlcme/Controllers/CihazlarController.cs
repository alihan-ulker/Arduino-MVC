using System.Web;//Add References -> Choose assemblies  
using System.Data;//Add  
using WA.OdaSicakligiOlcme.Models; // Add  
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;



namespace WA.OdaSicakligiOlcme.Controllers
{
    public class CihazlarController : Controller
    {
        private DBOdaSicakligiEntities db = new DBOdaSicakligiEntities();

        // GET: Cihazlar
        public ActionResult Index()
        {
            
            return View(db.Cihazlar.ToList());
        }

        // GET: Cihazlar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cihazlar cihazlar = db.Cihazlar.Find(id);
            if (cihazlar == null)
            {
                return HttpNotFound();
            }
            return View(cihazlar);
        }

        // GET: Cihazlar/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cihazlar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CihazID,Adi")] Cihazlar cihazlar)
        {
            if (ModelState.IsValid)
            {
                db.Cihazlar.Add(cihazlar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cihazlar);
        }

        // GET: Cihazlar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cihazlar cihazlar = db.Cihazlar.Find(id);
            if (cihazlar == null)
            {
                return HttpNotFound();
            }
            return View(cihazlar);
        }

        // POST: Cihazlar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CihazID,Adi")] Cihazlar cihazlar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cihazlar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cihazlar);
        }

        // GET: Cihazlar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cihazlar cihazlar = db.Cihazlar.Find(id);
            if (cihazlar == null)
            {
                return HttpNotFound();
            }
            return View(cihazlar);
        }

        // POST: Cihazlar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cihazlar cihazlar = db.Cihazlar.Find(id);
            db.Cihazlar.Remove(cihazlar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Sicaklik(int? id)
        {
            Cihazlar cihaz = new Cihazlar();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                cihaz = db.Cihazlar.First(p => p.CihazID == id);

                string URL = cihaz.Adi + ":80";
                XmlTextReader XMLOku = new XmlTextReader(URL);

                XMLOku.Close();
                XMLOku.Dispose();
                XMLOku = new XmlTextReader(URL);
                string Sicaklik = "", Nem = "", Son = "";
                while (XMLOku.Read())
                {
                    if (XMLOku.NodeType == XmlNodeType.Element && XMLOku.Name == "sicaklik")
                    {
                        XMLOku.Read();
                        Sicaklik = XMLOku.Value;
                    }

                    else if (XMLOku.NodeType == XmlNodeType.Element && XMLOku.Name == "nem")
                    {
                        XMLOku.Read();
                        Nem = XMLOku.Value;
                        Son = "Sıcaklık: " + Sicaklik + "C  " + "Nem: " + Nem + "% ";

                    }

                }
                ViewBag.Sicaklik = Sicaklik;
                ViewBag.Nem = Nem;
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
