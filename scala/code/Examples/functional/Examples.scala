
object Functional {
  class PipedObject[T] private[Functional] (value: T) {
    def |>[R](f: T => R) = f(this.value)
  }
  implicit def toPiped[T](value: T) = new PipedObject[T](value)
}

object Examples {
  def square(x: Int) = x * x

  def range(a: Int, b: Int): List[Int] = (for (i <- a to b) yield i).toList

  def map(f: Int => Int, list: List[Int]): List[Int] =
    if (list.isEmpty) List()
    else (f(list.head) :: map(f, list.tail))

  def reduce(f: (Int, Int) => Int, acc: Int, list: List[Int]): Int =
    if (list.isEmpty) acc
    else reduce(f, f(acc, list.head), list.tail)

  def |>(f: Int => Int, x: Int): Int = f(x)

  def readToEnd(filename: String): List[String] =
    {
      val brIn: java.io.BufferedReader = new java.io.BufferedReader(new java.io.FileReader(filename))
      var lines: List[String] = List()
      while (brIn.ready) { lines = (brIn.readLine) :: lines }

      lines
    }

  def crunch() =
    {
      val x = (readToEnd("data.csv") map { _.split(",") } map { _(0) } reverse).tail
      val x2 = x map { _.toDouble }
      val fw = new java.io.BufferedWriter(new java.io.FileWriter("result.csv"))
      x2 map { x => fw.write(x.toString + ",") }
      fw.close
    }
}