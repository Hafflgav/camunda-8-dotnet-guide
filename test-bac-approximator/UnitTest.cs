using blood_alcohol_approximator;

namespace test_bac_approximator;

public class Tests
{
    private Person testPersonMale;
    private Person testPersonFemale;

    [SetUp]
    public void Setup()
    {
        testPersonMale = new Person(75, "male");
        testPersonFemale = new Person(75, "women");
    }

    [Test]
    public void BloodAlcohol_CalculatedSuccessfully()
    {
        //When
        double bacMale = BloodAlcoholApproximator.Approximate(testPersonMale);
        double bacFemale = BloodAlcoholApproximator.Approximate(testPersonFemale);

        //Then
        Assert.That(bacMale, Is.Not.EqualTo(bacFemale));
        Assert.Less(bacFemale, bacMale);
    }

    [Test]
    public void DringSuggestion_SuggestsSomething()
    {
        //When
        Dictionary<String, double> drinks = BloodAlcoholApproximator.SuggestDrinks(65.00);

        //Then
        Assert.NotNull(drinks);
        Assert.IsNotEmpty(drinks);
    }
}
