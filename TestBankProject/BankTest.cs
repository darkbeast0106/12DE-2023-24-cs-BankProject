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
	}
}