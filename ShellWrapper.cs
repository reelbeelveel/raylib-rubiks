using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace rbx {

    class ShellException : Exception {
        public ShellException(string message) : base(message) { }
    }

    class ShellProcess {

        enum ShellProcessState {
            Idle,
            Started,
            Finished
        }

        public ShellProcess(string command, string? args = null, string? redirect = null) {
            this.command = command;
            this._args = args;
            this.redirect = redirect;
            this.procInfo = new ProcessStartInfo {
                FileName = command,
                Arguments = this.args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            this.state = ShellProcessState.Idle;
        }

        public bool IsRunning {
            get {
                return state == ShellProcessState.Started;
            }
        }

        public bool IsFinished {
            get {
                return state == ShellProcessState.Finished;
            }
            set {
                if (value && !IsFinished) {
                    _kill();
                    state = ShellProcessState.Finished;
                } else if(!value && IsFinished) {
                    throw new ShellException("Process is already finished");
                }
            }
        }
        
        public void Kill(){
            IsFinished = true;
        }

        public string? GetLine() {
            if(state == ShellProcessState.Finished) {
                return null;
            }
            if(state == ShellProcessState.Idle) {
                this.start();
            }
            if(this.proc.StandardOutput.EndOfStream) {
                this.state = ShellProcessState.Finished;
                return null;
            }
            string line = this.proc.StandardOutput.ReadLine();
            if(proc.StandardOutput.EndOfStream) {
                this.state = ShellProcessState.Finished;
            }
            using StreamWriter sw = new StreamWriter(redirect, append: true);
            sw.WriteLine(line);
            return line;
        }

        public void _continue() {
            GetLines();
        }

        public void _run() {
            start();
            GetLines();
        }

        public List<string> GetLines() {
            List<string> lines = new List<string>();
            string? line;
            while((line = GetLine()) != null) {
                lines.Add(line);
            }
            return lines;
        }

        public void Restart() {
            if(state != ShellProcessState.Idle)
                _kill();
            start();
        }

        private void _kill() {
            if(state == ShellProcessState.Idle) {
                throw new ShellException("Process is not running");
            }
            proc.Kill();
            state = ShellProcessState.Idle;
        }

        private void start() {
            if(state == ShellProcessState.Started) {
                throw new ShellException("Process already started");
            }
            proc = new Process{StartInfo = this.procInfo};
            proc.Start();
            state = ShellProcessState.Started;
        }


        public string args {
            get {
                return _args ?? "";
            }
            set {
                if(state != ShellProcessState.Idle) {
                    throw new ShellException("Process is already running");
                }
                _args = value;
                procInfo.Arguments = value;
            }
        }

        public string redirect {
            get {
                if(_redirect == null) {
                    _redirect = $"/tmp/{Guid.NewGuid().ToString()}";
                }
                return _redirect;
            }
            set {
                if(value == null) {
                    _redirect = null;
                } else {
                    _redirect = $"/tmp/{value}";
                }
            }
        }

        ~ShellProcess() {
            try {
                _kill();
            } catch(ShellException e) {
                // ignore
            }
        }

        private string command;
        private string? _args;
        private string? _redirect;
        private Process? proc;
        private ProcessStartInfo procInfo;
        private ShellProcessState state;
    }
}
