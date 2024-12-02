using BankProject;

namespace TestBankProject
{
	public class BankTest
	{
		Bank bank;

		[SetUp]
		public void Setup()
		{
			bank = new Bank();
		}

		[Test]
		public void UjSzamla_ErvenyesAdatokkal_ASzamlaEgyenlege0()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.That(bank.Egyenleg("1234"), Is.Zero);
		}

		[Test]
		public void UjSzamla_NullNev_ArgumentNullExceptiontDob()
		{
			Assert.Throws<ArgumentNullException>(() => bank.UjSzamla(null, "5678"));
		}

		[Test]
		public void UjSzamla_NullSzamlaszam_ArgumentNullExceptiontDob()
		{
			Assert.Throws<ArgumentNullException>(() => bank.UjSzamla("Teszt Elek", null));
		}

		[Test]
		public void UjSzamla_UresNev_ArgumentExceptiontDob()
		{
			Assert.Throws<ArgumentException>(() => bank.UjSzamla("", "5678"));
		}

		[Test]
		public void UjSzamla_UresSzamlaszam_ArgumentExceptiontDob()
		{
			Assert.Throws<ArgumentException>(() => bank.UjSzamla("Teszt Elek", ""));
		}

		[Test]
		public void UjSzamla_LetezoSzamlaszammal_ArgumentExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.Throws<ArgumentException>(() => bank.UjSzamla("Teszt Elek", "1234"));
		}

		[Test]
		public void UjSzamla_LetezoNevvel_NemDobKivetelt()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.DoesNotThrow(() => bank.UjSzamla("Gipsz Jakab", "5678"));
		}

		[Test]
		public void Egyenleg_NullSzamlaszammal_ArgumentNullExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.Throws<ArgumentNullException>(() => bank.Egyenleg(null));
		}

		[Test]
		public void Egyenleg_UresSzamlaszammal_ArgumentExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.Throws<ArgumentException>(() => bank.Egyenleg(""));
		}

		[Test]
		public void Egyenleg_NemLetezoSzamlaszammal_HibasSzamlaszamExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.Throws<HibasSzamlaszamException>(() => bank.Egyenleg("5678"));
		}

		[Test]
		public void Egyenleg_LetezoSzamlaszammal_NemDobKivetelt()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.DoesNotThrow(() => bank.Egyenleg("1234"));
		}


		[Test]
		public void EgyenlegFeltolt_NullSzamlaszammal_ArgumentNullExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.Throws<ArgumentNullException>(() => bank.EgyenlegFeltolt(null, 10000));
		}

		[Test]
		public void EgyenlegFeltolt_UresSzamlaszammal_ArgumentExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.Throws<ArgumentException>(() => bank.EgyenlegFeltolt("", 10000));
		}

		[Test]
		public void EgyenlegFeltolt_NemLetezoSzamlaszammal_HibasSzamlaszamExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.Throws<HibasSzamlaszamException>(() => bank.EgyenlegFeltolt("5678", 10000));
		}

		[Test]
		public void EgyenlegFeltolt_NullaOsszeggel_ArgumentExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			Assert.Throws<ArgumentException>(() => bank.EgyenlegFeltolt("1234", 0));
		}

		[Test]
		public void EgyenlegFeltolt_LetezoSzamlaraErvenyesOszeggel_AzEgyenlegMegvaltozik()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			bank.EgyenlegFeltolt("1234", 10000);

			Assert.That(bank.Egyenleg("1234"), Is.EqualTo(10000));
		}

		[Test]
		public void EgyenlegFeltolt_TobbszoriFeltoltesEseten_AzEgyenlegOsszeadodik()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");

			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("1234", 20000);
			bank.EgyenlegFeltolt("1234", 15000);

			Assert.That(bank.Egyenleg("1234"), Is.EqualTo(45000));
		}

		[Test]
		public void EgyenlegFeltolt_TobbSzamlaEseten_MegfeleloSzamlaraToltiAzOsszeget()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");

			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.That(bank.Egyenleg("1234"), Is.EqualTo(10000));
			Assert.That(bank.Egyenleg("5678"), Is.EqualTo(20000));
			Assert.That(bank.Egyenleg("9876"), Is.EqualTo(15000));
		}

		[Test]
		public void Utal_ErvenyesAdatokkal_SikeresUtalas()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			bool eredmeny = bank.Utal("1234", "9876", 5000);
			Assert.That(eredmeny, Is.True);
			Assert.That(bank.Egyenleg("1234"), Is.EqualTo(5000));
			Assert.That(bank.Egyenleg("9876"), Is.EqualTo(20000));
		}

		[Test]
		public void Utal_HonnanNull_ArgumentNullExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.Throws<ArgumentNullException>(() => bank.Utal(null, "9876", 5000));
		}

		[Test]
		public void Utal_HovaNull_ArgumentNullExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.Throws<ArgumentNullException>(() => bank.Utal("1234", null, 5000));
		}

		[Test]
		public void Utal_HonnanUres_ArgumentExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.Throws<ArgumentException>(() => bank.Utal("", "9876", 5000));
		}

		[Test]
		public void Utal_HovaUres_ArgumentExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.Throws<ArgumentException>(() => bank.Utal("1234", "", 5000));
		}

		[Test]
		public void Utal_HonnanNemLetezik_HibasSzamlaszamExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.Throws<HibasSzamlaszamException>(() => bank.Utal("2468", "9876", 5000));
		}

		[Test]
		public void Utal_HovaNemLetezik_HibasSzamlaszamExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.Throws<HibasSzamlaszamException>(() => bank.Utal("1234", "2468", 5000));
		}

		[Test]
		public void Utal_0Osszeg_ArgumentExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.Throws<ArgumentException>(() => bank.Utal("1234", "9876", 0));
		}

		[Test]
		public void Utal_HonnanEsHovaMegegyezik_ArgumentExceptiontDob()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			Assert.Throws<ArgumentException>(() => bank.Utal("1234", "1234", 5000));
		}

		[Test]
		public void Utal_SzamlanLevoTeljesOsszeg_SikeresUtalas()
		{
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			bool eredmeny = bank.Utal("1234", "9876", 10000);
			Assert.That(eredmeny, Is.True);
			Assert.That(bank.Egyenleg("1234"), Is.EqualTo(0));
			Assert.That(bank.Egyenleg("9876"), Is.EqualTo(25000));
		}

		[Test]
		public void Utal_NagyobbOsszeggMintAmiASzamlanVan_SikertelenUtalasEgyenlegekNemValtoznak()
		{
			// Arrange
			bank.UjSzamla("Gipsz Jakab", "1234");
			bank.UjSzamla("Gipsz Jakab", "5678");
			bank.UjSzamla("Teszt Elek", "9876");
			bank.EgyenlegFeltolt("1234", 10000);
			bank.EgyenlegFeltolt("5678", 20000);
			bank.EgyenlegFeltolt("9876", 15000);

			// Act
			bool eredmeny = bank.Utal("1234", "9876", 10001);

			// Assert
			Assert.That(eredmeny, Is.False);
			Assert.That(bank.Egyenleg("1234"), Is.EqualTo(10000));
			Assert.That(bank.Egyenleg("9876"), Is.EqualTo(15000));
		}
	}
}