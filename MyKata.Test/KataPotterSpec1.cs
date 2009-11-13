 
using System;
using System.Collections.Generic;
using System.Linq;
using developwithpassion.bdd.contexts;
using developwithpassion.bdd.harnesses.mbunit;
using developwithpassion.bdddoc.core;
/*
 * If you want to try this Kata for yourself or at your dojo meeting, read the problem description and see if the Kata appeals to you. The rest is commentary and helpful clues for if you get stuck solving it. I would recommend trying the Kata for yourself before reading too much of it.

Problem Description

Once upon a time there was a series of 5 books about a very English hero called Harry. (At least when this Kata was invented, there were only 5. Since then they have multiplied) Children all over the world thought he was fantastic, and, of course, so did the publisher. So in a gesture of immense generosity to mankind, (and to increase sales) they set up the following pricing model to take advantage of Harry's magical powers.

One copy of any of the five books costs 8 EUR. If, however, you buy two different books from the series, you get a 5% discount on those two books. If you buy 3 different books, you get a 10% discount. With 4 different books, you get a 20% discount. If you go the whole hog, and buy all 5, you get a huge 25% discount.

Note that if you buy, say, four books, of which 3 are different titles, you get a 10% discount on the 3 that form part of a set, but the fourth book still costs 8 EUR.

Potter mania is sweeping the country and parents of teenagers everywhere are queueing up with shopping baskets overflowing with Potter books. Your mission is to write a piece of code to calculate the price of any conceivable shopping basket, giving as big a discount as possible.

For example, how much does this basket of books cost?

  2 copies of the first book
  2 copies of the second book
  2 copies of the third book
  1 copy of the fourth book
  1 copy of the fifth book

(answer: 51.20 EUR)

Clues

You’ll find that this Kata is easy at the start. You can get going with tests for baskets of 0 books, 1 book, 2 identical books, 2 different books… and it is not too difficult to work in small steps and gradually introduce complexity.

However, the twist becomes apparent when you sit down and work out how much you think the sample basket above should cost. It isn’t 5*8*0.75+3*8*0.90. It is in fact 4*8*0.8+4*8*0.8. So the trick with this Kata is not that the acceptance test you’ve been given is wrong. The trick is that you have to write some code that is intelligent enough to notice that two sets of four books is cheaper than a set of five and a set of three.

You will have to introduce a certain amount of clever optimization algorithm. But not too much! This problem does not require a fully fledged general purpose optimizer. Try to solve just this problem well in order to share it for everyone or even in the [chat line]. Trust that you can generalize and improve your solution if and when new requirements come along.

Suggested Test Cases

(Originally posted at xp-france.net/cgi-bin/wiki.pl?KataPotter)

def testBasics
  assert_equal(0, price([]))
  assert_equal(8, price([0]))
  assert_equal(8, price([1]))
  assert_equal(8, price([2]))
  assert_equal(8, price([3]))
  assert_equal(8, price([4]))
  assert_equal(8 * 2, price([0, 0]))
  assert_equal(8 * 3, price([1, 1, 1]))
end

def testSimpleDiscounts
  assert_equal(8 * 2 * 0.95, price([0, 1]))
  assert_equal(8 * 3 * 0.9, price([0, 2, 4]))
  assert_equal(8 * 4 * 0.8, price([0, 1, 2, 4]))
  assert_equal(8 * 5 * 0.75, price([0, 1, 2, 3, 4]))
end

def testSeveralDiscounts
  assert_equal(8 + (8 * 2 * 0.95), price([0, 0, 1]))
  assert_equal(2 * (8 * 2 * 0.95), price([0, 0, 1, 1]))
  assert_equal((8 * 4 * 0.8) + (8 * 2 * 0.95), price([0, 0, 1, 2, 2, 3]))
  assert_equal(8 + (8 * 5 * 0.75), price([0, 1, 1, 2, 3, 4]))
end

def testEdgeCases
  assert_equal(2 * (8 * 4 * 0.8), price([0, 0, 1, 1, 2, 2, 3, 4]))
  assert_equal(3 * (8 * 5 * 0.75) + 2 * (8 * 4 * 0.8), 
    price([0, 0, 0, 0, 0, 
           1, 1, 1, 1, 1, 
           2, 2, 2, 2, 
           3, 3, 3, 3, 3, 
           4, 4, 4, 4]))
end

 */
namespace MyKata.Test
{
    public class KataPotterSpec1
    {
        public abstract class concern : observations_for_a_static_sut {}

        [Concern(typeof(PotterBookBasketPriceCalculator))]
        public class when_basket_contains_no_book : concern
        {
            it price_should_be_ = () =>
            {
                new PotterBookBasket(new List<PotterBook>()).CalculatePrice().should_be_equal_to(0);
            };
        }

        [Concern(typeof (PotterBookBasketPriceCalculator))]
        public class when_basket_contains_only_one_book : concern
        {
            it price_should_be_8 = () =>
            {
                new PotterBookBasket(new[] { PotterBook.CreateBook(1) }).CalculatePrice().should_be_equal_to(8);
                new PotterBookBasket(new[] { PotterBook.CreateBook(2) }).CalculatePrice().should_be_equal_to(8);
                new PotterBookBasket(new[] { PotterBook.CreateBook(3) }).CalculatePrice().should_be_equal_to(8);
                new PotterBookBasket(new[] { PotterBook.CreateBook(4) }).CalculatePrice().should_be_equal_to(8);
                new PotterBookBasket(new[] { PotterBook.CreateBook(5) }).CalculatePrice().should_be_equal_to(8);
            };
        }

        [Concern(typeof(PotterBookBasketPriceCalculator))]
        public class when_basket_contains_only_books_of_the_same_title : concern
        {
            it price_should_be_8_time_count_of_books = () =>
            {
                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(1)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 2);

                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(2),
                        PotterBook.CreateBook(2),
                        PotterBook.CreateBook(2)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 3);

            };
        }


        [Concern(typeof(PotterBookBasketPriceCalculator))]
        public class when_basket_contains_only_books_of_different_titles : concern
        {
            it price_should_be_8_time_count_of_books_and_then_apply_discount_rate = () =>
            {
                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(2)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 2 * 0.95);

                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(2),
                        PotterBook.CreateBook(3)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 3 * .9);

                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(2),
                        PotterBook.CreateBook(3),
                        PotterBook.CreateBook(4)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 4 * .8);

                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(2),
                        PotterBook.CreateBook(3),
                        PotterBook.CreateBook(4),
                        PotterBook.CreateBook(5)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 5 * .75);

            };
        }

        /*
def testSeveralDiscounts
  assert_equal(8 + (8 * 2 * 0.95), price([0, 0, 1]))
  assert_equal(2 * (8 * 2 * 0.95), price([0, 0, 1, 1]))
  assert_equal((8 * 4 * 0.8) + (8 * 2 * 0.95), price([0, 0, 1, 2, 2, 3]))
  assert_equal(8 + (8 * 5 * 0.75), price([0, 1, 1, 2, 3, 4]))
end
         */
        [Concern(typeof(PotterBookBasketPriceCalculator))]
        public class when_basket_contains_mixed_books : concern
        {
            it price_should_be_8_time_count_of_books_and_then_apply_discount_rate = () =>
            {
                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(2)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 2 * 0.95);

                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(2),
                        PotterBook.CreateBook(3)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 3 * .9);

                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(2),
                        PotterBook.CreateBook(3),
                        PotterBook.CreateBook(4)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 4 * .8);

                new PotterBookBasket(
                    new[] { 
                        PotterBook.CreateBook(1),
                        PotterBook.CreateBook(2),
                        PotterBook.CreateBook(3),
                        PotterBook.CreateBook(4),
                        PotterBook.CreateBook(5)
                    }
                    ).CalculatePrice().should_be_equal_to(8 * 5 * .75);

            };
        }
    }

    public class PotterBookBasketPriceCalculator 
    {
    }

    public class PotterBookBasket
    {
        readonly IEqualityComparer<PotterBook> _comparer;
        List<PotterBook> _books;

        public PotterBookBasket(IEnumerable<PotterBook> books, IEqualityComparer<PotterBook> comparer)
        {
            _comparer = comparer;
            _books = new List<PotterBook>(books);
        }

        public PotterBookBasket(IEnumerable<PotterBook> books): this(books, new PotterBookComparer())
        {
            _books = new List<PotterBook>(books);
        }

        public double CalculatePrice()
        {
            if (_books.Count == 0) return 0;
            if (_books.Count == 1) return _books[0].Price;
            if (_books.AreAllTheSameTitle(_comparer)) return _books.Count * _books[0].Price;
            if (_books.AreAllDifferentTitle(_comparer)) return _books.Count * _books[0].Price * discount(_books.Count);

            return -100;
        }

        double discount(int count)
        {
            if (count == 1) return 1;
            if (count == 2) return .95;
            if (count == 3) return .9;
            if (count == 4) return .8;
            return .75;
        }
    }

    public static class EnumerableExtension
    {
        public static bool AreAllTheSameTitle<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            return enumerable.Distinct(comparer).Count() == 1;
        }
        public static bool AreAllDifferentTitle<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            return enumerable.Distinct(comparer).Count() == enumerable.Count();
        }
    }

    public class PotterBookComparer : IEqualityComparer<PotterBook>
    {
        public bool Equals(PotterBook x, PotterBook y)
        {
            return x.Title.Equals(y.Title);
        }

        public int GetHashCode(PotterBook obj)
        {
            return obj.Title.GetHashCode();
        }
    }

    public class PotterBook: IEquatable<PotterBook>
    {
        string title;
        public double Price = 8;

        PotterBook(int i)
        {
            title = "Potter Book " + i;
        }

        public string Title
        {
            get { return title; }
        }

        static public PotterBook CreateBook(int i)
        {
            return new PotterBook(i);
        }

        public bool Equals(PotterBook other)
        {
            return this.Title.Equals(other.Title);
        }
    }
}
