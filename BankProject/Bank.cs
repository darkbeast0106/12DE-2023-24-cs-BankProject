﻿namespace BankProject
{
    /// <summary>
    /// Bank műveleteit végrehajtó osztály.
    /// </summary>
    public class Bank
    {
        private class Szamla
        {
            public Szamla(string nev, string szamlaszam) { 
                this.Nev = nev;
                this.Szamlaszam = szamlaszam;
                this.Egyenleg = 0;
            }
            public string Nev { get; set; }
            public string Szamlaszam { get; set; }
            public ulong Egyenleg { get; set; }
        }

        private List<Szamla> szamlak = new List<Szamla>();

        /// <summary>
        /// Új számlát nyit a megadott névvel, számlaszámmal, 0 Ft egyenleggel
        /// </summary>
        /// <param name="nev">A számla tulajdonosának neve</param>
        /// <param name="szamlaszam">A számla számlaszáma</param>
        /// <exception cref="ArgumentNullException">A név és a számlaszám nem lehet null</exception>
        /// <exception cref="ArgumentException">A név és a számlaszám nem lehet üres
        /// A számlaszámmal nem létezhet számla
        /// A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
        public void UjSzamla(string nev, string szamlaszam)
        {
            if (nev == null)
            {
                throw new ArgumentNullException(nameof(nev));
            }
			if (szamlaszam == null)
			{
				throw new ArgumentNullException(nameof(szamlaszam));
			}
			if (nev == "")
			{
				throw new ArgumentException("A név nem lehet üres", nameof(nev));
			}
			if (szamlaszam == "")
			{
				throw new ArgumentException("A számlaszám nem lehet üres", nameof(szamlaszam));
			}

            int index = 0;
            while (index < szamlak.Count && !szamlak[index].Szamlaszam.Equals(szamlaszam))
            {
                index++;
            }
            if (index < szamlak.Count)
            {
                throw new ArgumentException("A megadott számlaszámmal már létezik számla", nameof(szamlaszam));
            }
            
            szamlak.Add(new Szamla(nev, szamlaszam));
        }

        /// <summary>
        /// Lekérdezi az adott számlán lévő pénzösszeget
        /// </summary>
        /// <param name="szamlaszam">A számla számlaszáma, aminek az egyenlegét keressük</param>
        /// <returns>A számlán lévő egyenleg</returns>
        /// <exception cref="ArgumentNullException">A számlaszám nem lehet null</exception>
        /// <exception cref="ArgumentException">A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
        /// <exception cref="HibasSzamlaszamException">A megadott számlaszámmal nem létezik számla</exception>
        public ulong Egyenleg(string szamlaszam)
		{
			Szamla szamla = SzamlaKereses(szamlaszam);

			return szamla.Egyenleg;
		}

		/// <summary>
		/// Egy létező számlára pénzt helyez
		/// </summary>
		/// <param name="szamlaszam">A számla számlaszáma, amire pénzt helyez</param>
		/// <param name="osszeg">A számlára helyezendő pénzösszeg</param>
		/// <exception cref="ArgumentNullException">A számlaszám nem lehet null</exception>
		/// <exception cref="ArgumentException">Az összeg csak pozitív lehet.
		/// A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
		/// <exception cref="HibasSzamlaszamException">A megadott számlaszámmal nem létezik számla</exception>
		public void EgyenlegFeltolt(string szamlaszam, ulong osszeg)
        {
            if (osszeg == 0)
            {
                throw new ArgumentException("Az összeg csak pozitív egész szám lehet", nameof(osszeg));
            }

			Szamla szamla = SzamlaKereses(szamlaszam);

			szamla.Egyenleg += osszeg;
        }

		private Szamla SzamlaKereses(string szamlaszam)
		{
			if (szamlaszam == null)
			{
				throw new ArgumentNullException(nameof(szamlaszam));
			}
			if (szamlaszam == "")
			{
				throw new ArgumentException("A számlaszám nem lehet üres", nameof(szamlaszam));
			}

			int index = 0;
			while (index < szamlak.Count && !szamlak[index].Szamlaszam.Equals(szamlaszam))
			{
				index++;
			}
			if (index == szamlak.Count)
			{
				throw new HibasSzamlaszamException(szamlaszam);
			}

			return szamlak[index];
		}


		/// <summary>
		/// Két számla között utal.
		/// Ha nincs elég pénz a forrás számlán, akkor false értékkel tér vissza
		/// </summary>
		/// <param name="honnan">A forrás számla számlaszáma</param>
		/// <param name="hova">A cél számla számlaszáma</param>
		/// <param name="osszeg">Az átutalandó egyenleg</param>
		/// <returns>Az utalás sikeressége</returns>
		/// <exception cref="ArgumentNullException">A forrás és cél számlaszám nem lehet null</exception>
		/// <exception cref="ArgumentException">Az összeg csak pozitív lehet.
		/// A számlaszám számot, szóközt és kötőjelet tartalmazhat</exception>
		/// <exception cref="HibasSzamlaszamException">A megadott számlaszámmal nem létezik számla</exception>
		public bool Utal(string honnan, string hova, ulong osszeg)
        {
            if (osszeg == 0)
			{
				throw new ArgumentException("Az összeg csak pozitív egész szám lehet", nameof(osszeg));
			}
            if (honnan == hova)
            {
                throw new ArgumentException("A forrás és a cél számlaszám nem egyezhet meg", nameof(hova));
            }
            Szamla forrasSzamla = SzamlaKereses(honnan);
            Szamla celSzamla = SzamlaKereses(hova);

            if (forrasSzamla.Egyenleg >= osszeg)
            {
                forrasSzamla.Egyenleg -= osszeg;
                celSzamla.Egyenleg += osszeg;
                return true;
            }
            return false;
        }
    }
}