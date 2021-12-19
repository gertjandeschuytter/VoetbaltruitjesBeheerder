namespace BusinessLayer.Model
{
    public interface IVoetbaltruitje
    {
        Club Club { get; }
        ClubSet ClubSet { get; }
        int Id { get; }
        Kledingmaat Kledingmaat { get; set; }
        double Prijs { get; }
        string Seizoen { get; }

        void ZetClub(Club club);
        void ZetClubSet(ClubSet clubSet);
        void ZetId(int id);
        void ZetPrijs(double prijs);
        void ZetSeizoen(string seizoen);
    }
}