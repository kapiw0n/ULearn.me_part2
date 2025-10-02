using System;
using System.Text;
namespace hashes
{
    public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
        IMagic
    {
        Cat ghostCat;
        Document doc;
        byte[] docContent = Encoding.ASCII.GetBytes("It's a doc");
        byte[] hackerMessage = Encoding.ASCII.GetBytes("I'm hacker");
        Vector vec;
        Segment seg;
        Robot robo;

        public void DoMagic()
        {
            ghostCat?.Rename("GhostCat");
            Robot.BatteryCapacity = 100500;
            vec?.Add(vec);
            seg?.Start?.Add(seg.Start);
            if (doc != null)
                for (int i = 0; i < docContent.Length; i++)
                    docContent[i] = hackerMessage[i];
        }

        Vector IFactory<Vector>.Create()
        {
            if (vec == null)
                vec = new Vector(42, 42);
            return vec;
        }

        Segment IFactory<Segment>.Create()
        {
            if (seg == null)
                seg = new Segment(new Vector(42, 42), new Vector(42, 42));
            return seg;
        }

        Document IFactory<Document>.Create()
        {
            if (doc == null)
                doc = new Document("NewDoc", Encoding.UTF8, docContent);
            return doc;
        }

        Cat IFactory<Cat>.Create()
        {
            if (ghostCat == null)
                ghostCat = new Cat("Kitty", "yeee", DateTime.Now);
            return ghostCat;
        }

        Robot IFactory<Robot>.Create()
        {
            Robot.BatteryCapacity = 100;
            if (robo == null)
                robo = new Robot("0_0");
            return robo;
        }
    }
}