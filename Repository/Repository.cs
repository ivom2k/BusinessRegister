using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Repository
    {
        private readonly string _filePath;
        

        public Repository(string filePath)
        {
            _filePath = filePath;
        }

        public Company GetCompany(string companyCode)
        {
            
            
            return new Company();
        }

        public List<Company> GetCompanies(string company)
        {

            return new List<Company>();
        }

        public async Task<Task> ProcessLinesAsync()
        {
            var pipe = new Pipe();
            await using var fileStream = File.OpenRead(_filePath);
            
            Task writing = FillPipeAsync(fileStream, pipe.Writer);
            Task reading = ReadPipeAsync(pipe.Reader);
            
            
            return Task.WhenAll(reading, writing);
        }
        
        public async Task ReadFile()
        {
            
            var separator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            Console.WriteLine("filePath: " + _filePath);
            Console.WriteLine("separator " + separator);
            
            try
            {
                using StreamReader streamReader = new StreamReader(_filePath, Encoding.UTF8);
                for (var i = 0; i < 5; i++)
                {
                    Console.WriteLine(await streamReader.ReadLineAsync());
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
        }

        async Task FillPipeAsync(FileStream fs, PipeWriter writer)
        {
            // FillPipeAsync reads from the FileStream (file) and writes into the PipeWriter.
            const int minimumBufferSize = 512;

            while (true)
            {
                // Allocate at least 512 bytes from the PipeWriter
                Memory<byte> memory = writer.GetMemory(minimumBufferSize);

                try
                {
                    // int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
                    var bytesRead = await fs.ReadAsync(memory);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    
                    // Tell the PipeWriter how much was read from the Stream
                    writer.Advance(bytesRead);
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Reading bytes failed! ", e);
                }
                
                
                FlushResult flushResult = await writer.FlushAsync();

                if (flushResult.IsCompleted)
                {
                    break;
                }

                // Tell the PipeReader that there's no more data coming
                await writer.CompleteAsync();
            }
        }

        async Task ReadPipeAsync(PipeReader reader)
        {
            // ReadPipeAsync reads from the PipeReader and parses incoming lines.
            while (true)
            {
                ReadResult result = await reader.ReadAsync();

                ReadOnlySequence<byte> buffer = result.Buffer;
                SequencePosition? position = null;

                do
                {
                    // Look for a EOL in the buffer
                    position = buffer.PositionOf((byte)'\n');
                    
                    if (position != null)
                    {
                        ProcessLine(buffer.Slice(0, position.Value));
                        
                        // Skip the line + the \n character (basically position)
                        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                    }
                    
                } while (position != null);
                
                // Tell the PipeReader how much of the buffer we have consumed
                // AdvanceTo tells the PipeReader that these buffers are no longer required by the reader so they can be discarded
                reader.AdvanceTo(buffer.Start, buffer.End);
                
                // Stop reading if there's no more data coming
                if (result.IsCompleted)
                {
                    break;
                }
            }
            // Mark the PipeReader as complete
            await reader.CompleteAsync();
        }
        
        private void ProcessLine(ReadOnlySequence<byte> sequence)
        {
            
            Encoding.UTF8.GetString(sequence);
        }
        
    }
}