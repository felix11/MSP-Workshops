
import scala.actors.Actor

case class Download(handler:Array[Byte] => Unit, url:String)
case class Stop

class DataLoader() extends Actor {
  def act() {
	  loop
	  {
	 	  react
	 	  {
	 	 	  case Download(handler, url) =>
			    App.load(Download(handler,url))
	 	 	  case Stop =>
			    exit()
	 	  }
	  }
  }
}

class ComputingEngine extends Actor {
	def act() {
		loop
		{
			react
			{
				case (data:String) =>
					println(data.split("\n").length)//data.split("\n") map { line => line.split(",") map { _(0) } map { x => println(x) } }
					data.split("\n") filter { line => line.length > 10 } map { line => println(line) }
				    exit()
			}
		}
	}
}

object App {
	
	def load(dl:Download) =
	{
		val handler = dl.handler 
		val url = dl.url 
		
		val in: java.io.BufferedInputStream = new java.io.BufferedInputStream(new java.net.URL(url).openStream());
			
	    val baos: java.io.ByteArrayOutputStream = new java.io.ByteArrayOutputStream();
	    val bout: java.io.BufferedOutputStream = new java.io.BufferedOutputStream(baos, 1024);
	    val data: Array[Byte] = new Array[Byte](1024);
	    while (in.read(data, 0, 1024) >= 0)
	    {
	      bout.write(data);
	    }
	    bout.close();
	    in.close();
	    handler(baos.toByteArray)
	}
	
	def loadMultiple()
	{
	  val loaderN = 10
      val loader : List[DataLoader] = (for {i <- 0 until loaderN} yield new DataLoader()).toList
      
      val dataHandler = (data:Array[Byte]) => println(new String(data))//println("download finished, loaded " + (new String(data)).length + " chars.")

      val d1 = Download(dataHandler, "http://ichart.finance.yahoo.com/table.csv?s=MSFT&d=2&e=23&f=2011&g=d&a=2&b=13&c=1986&ignore=.csv")
      val d2 = Download(dataHandler, "http://ichart.finance.yahoo.com/table.csv?s=GOOG&d=2&e=23&f=2011&g=d&a=7&b=19&c=2004&ignore=.csv")
      
      val t1 = System.currentTimeMillis
      //loader map { l => l.start; l ! d1; l ! Stop }
      for {i<- 1 to loaderN} load(d1)
	}
	
	def loadData()
	{
      val loader : DataLoader = new DataLoader()
      
      val dataHandler = (data:Array[Byte]) =>
      {
    	  val c = new ComputingEngine()
    	  c.start
    	  c ! new String(data)
      }

      val micDownload = Download(dataHandler, "http://ichart.finance.yahoo.com/table.csv?s=MSFT&d=2&e=23&f=2011&g=d&a=2&b=13&c=1986&ignore=.csv")
      val gogDownload = Download(dataHandler, "http://ichart.finance.yahoo.com/table.csv?s=GOOG&d=2&e=23&f=2011&g=d&a=7&b=19&c=2004&ignore=.csv")
      
      loader.start
      
      loader ! micDownload
      loader ! gogDownload
      loader ! Stop
	}

  def main(args: Array[String]): Unit =
    {
	  //loadMultiple()
	  loadData()
    }

}