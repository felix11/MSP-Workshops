
namespace ZPASim

open System.Windows.Forms.DataVisualization.Charting

type Simulation() =
    let rand = new System.Random()
    let startRandomActions (student:Students.Student) =
                student.Post(Students.Login)
                student.Post(Students.Register(rand.Next(1,4)))
                student.Post(Students.Logout)

    member this.start(studentcount, timepoints:DataPointCollection, datapoints:DataPointCollection, loaders:System.Collections.Concurrent.ConcurrentQueue<bool>) =
        let studentlist = [ for i in 1 .. studentcount do yield new ZPASim.Students.Student(i) ]
        studentlist |> List.map (fun s ->
                        s.Sample.Add(fun (time,data) ->
                                do timepoints.AddXY(s.id, time.TotalMilliseconds)  |> ignore
                                do datapoints.AddXY(s.id, data)                    |> ignore)
                        s.Message.Add(fun (msg) ->
                                if msg = "load" then
                                    do loaders.Enqueue(true)
                                else
                                    do loaders.TryDequeue() |> ignore)
                        s.Start())
                    |> ignore
        //System.Threading.Thread.Sleep(2000)
        studentlist |> List.map (fun s -> startRandomActions s)
                    |> ignore
        ()

    member this.results() = "No results yet."