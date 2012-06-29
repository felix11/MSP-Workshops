namespace ZPASim

module Students =
    open System.Collections.Generic
    open System.Threading
    open System.Net
    open System.IO

    type Agent<'T> = MailboxProcessor<'T>

    type Work =
        | Login
        | Logout
        | Register of int

    let baseUrl = "http://localhost:1824/"//"http://news.google.com/?"//
    let request (url:string) = (new WebClient()).DownloadString(url)

    let decide x =
        match x with
        | Login ->
            request (baseUrl + "?todo=login")
        | Logout ->
            request (baseUrl + "Default.aspx?todo=logout")
        | Register(course) ->
            request (baseUrl + "Courses.aspx?todo=register&course=" + course.ToString())

    type SimData = System.TimeSpan * int
            
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

    // Modified from
    // http://blogs.msdn.com/b/dsyme/archive/2010/02/15/async-and-parallel-design-patterns-in-f-part-3-agents.aspx
    type Student(number:int) =
        let sample = new Event<SimData>()
        let message = new Event<string>()

        let mutable filecounter = 0

        // Capture the synchronization context to allow us to raise events
        // back on the GUI thread
        let syncContext = SynchronizationContext.CaptureCurrent()

        // The internal mailbox processor agent
        let agent =
            new MailboxProcessor<_>(fun inbox ->
                async { while true do
                            let! msg = inbox.Receive()
                            syncContext.RaiseEvent message ("load")
                            let tic = System.DateTime.Now
                            let result = (decide msg)
                            let toc = ((System.DateTime.Now - tic))
                            syncContext.RaiseEvent sample (toc,result.Length)
                            syncContext.RaiseEvent message ("unload") })

        /// Post a message to the agent
        member x.Post msg = agent.Post msg

        /// Start the agent
        member x.Start () = agent.Start()

        member x.Sample = sample.Publish
        member x.Message = message.Publish

        member x.id = number