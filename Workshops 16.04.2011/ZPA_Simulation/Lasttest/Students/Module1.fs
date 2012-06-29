
namespace ZPA

module SimZPA =

    open System.Net
    open System.Threading

    type Work =
        | Login
        | Logout
        | Register of int

    type Agent<'T> = MailboxProcessor<'T>

    
            
    // Modified from
    // http://blogs.msdn.com/b/dsyme/archive/2010/01/10/async-and-parallel-design-patterns-in-f-reporting-progress-with-events-plus-twitter-sample.aspx
    type SynchronizationContext with
        /// A standard helper extension method to raise an event on the GUI thread
        member syncContext.RaiseEvent (event: Event<_>) args =
            syncContext.Post((fun _ -> event.Trigger args),state=null)

        /// A standard helper extension method to capture the current synchronization context.
        /// If none is present, use a context that executes work in the thread pool.
        static member CaptureCurrent () =
            match SynchronizationContext.Current with
            | null -> new SynchronizationContext()
            | ctxt -> ctxt
    
    type Student(name,loaders:System.Collections.Concurrent.ConcurrentQueue<bool>) =

        let message = new Event<string>()

        // Capture the synchronization context to allow us to raise events
        // back on the GUI thread
        let syncContext = SynchronizationContext.CaptureCurrent()

        let baseUrl = "http://localhost:1824/"
        let requestUrl (url:string) = (new WebClient()).DownloadString(url)

        let decide command = 
            match command with
            | Login -> requestUrl(baseUrl + "?todo=login")
            | Logout ->  requestUrl(baseUrl + "?todo=logout")
            | Register(course) ->  requestUrl(baseUrl + "Courses.aspx?todo+register&course=" + course.ToString())

        let agent = new Agent<_>(fun inbox ->
                            async { while true do
                                        let! msg = inbox.Receive()
                                        loaders.Enqueue(true)
                                        let result =decide msg
                                        loaders.TryDequeue() |> ignore
                                        //System.Console.WriteLine(result.Length)
                                        syncContext.RaiseEvent message ("load")
                                        ()
                                  })

        member this.Post msg = agent.Post msg
        member this.Start() = agent.Start()
        member this.Message = message.Publish

    type Simulation() =

        let startRandomAction (student:Student) =
            student.Post(Work.Login)
            student.Post(Work.Register(2))
            student.Post(Work.Logout)
    
        member this.start(studentcount, loaders : System.Collections.Concurrent.ConcurrentQueue<bool>, textbox:System.Windows.Forms.TextBox) =
                let students = [ for i in 1 .. studentcount do yield new Student("#" + i.ToString(), loaders) ]
                students |> List.map( fun s -> s.Start() ) |> ignore
                students |> List.map( fun s -> s.Message.Add(fun msg -> textbox.Text <- textbox.Text + msg + System.Environment.NewLine) ) |> ignore
                students |> List.map (fun s -> startRandomAction s) |> ignore
                ()

