using System;
using System.Buffers;
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

        public async Task<Company> GetCompany(string companyCode)
        {
            return await ProcessCompanyCode(companyCode);
        }

        public List<Company> GetCompanies(string companyName)
        {

            return new List<Company>();
        }

        private async Task<Company> ProcessCompanyCode(string companyCode)
        {
            var pipe = new Pipe();
            await using var fileStream = File.OpenRead(_filePath);

            // Task writingToPipe = FillPipe(fileStream, pipe.Writer);
            // Task readingFromPipe = ReadPipe(pipe.Reader, companyCode);
            
            await FillPipe(fileStream, pipe.Writer);
            var result = ReadPipe(pipe.Reader, companyCode).Result;

            
            // await Task.WhenAll(writingToPipe, ReadPipe(pipe.Reader, companyCode));

            return result;
        }

        private async Task FillPipe(FileStream fs, PipeWriter writer)
        {
            while (true)
            {
                var memory = writer.GetMemory(512);
                
                try
                {
                    var bytesRead = await fs.ReadAsync(memory);
                    
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    
                    writer.Advance(bytesRead);
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Reading bytes failed! ", e);
                }
                
                var flushResult = await writer.FlushAsync();
            
                if (flushResult.IsCompleted)
                {
                    break;
                }
                
                await writer.CompleteAsync();
            }
        }

        private async Task<Company> ReadPipe(PipeReader reader, string companyCode)
        {
            Company resultCompany = new();

            while (true)
            {
                var result = await reader.ReadAsync();
                
                var buffer = result.Buffer;
                SequencePosition? position;

                do
                {
                    position = buffer.PositionOf((byte)'\n');

                    if (position != null)
                    {
                        resultCompany = ProcessCompanyCode(buffer.Slice(1, position.Value), companyCode);
                        
                        
                        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                    }
                    
                } while (position != null);
                
                reader.AdvanceTo(buffer.Start, buffer.End);
                
                if (result.IsCompleted)
                {
                    break;
                }
                
                await reader.CompleteAsync();
            }

            return resultCompany;
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

        private async Task FillPipeAsync(FileStream fs, PipeWriter writer)
        {
            // FillPipeAsync reads from the FileStream (file) and writes into the PipeWriter.
            const int minimumBufferSize = 512;

            while (true)
            {
                // Get some memory from the underlying writer
                // Allocate at least 512 bytes from the PipeWriter
                Memory<byte> memory = writer.GetMemory(minimumBufferSize);

                try
                {
                    
                    var bytesRead = await fs.ReadAsync(memory);
                    
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    
                    // Tell the PipeWriter how much was read from the Stream
                    // tell the PipeWriter how much data we actually wrote to the buffer
                    writer.Advance(bytesRead);
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Reading bytes failed! ", e);
                }
                
                // make the data available to the PipeReader
                FlushResult flushResult = await writer.FlushAsync();

                if (flushResult.IsCompleted)
                {
                    break;
                }

                // Tell the PipeReader that there's no more data coming
                await writer.CompleteAsync();
            }
        }

        private async Task ReadPipeAsync(PipeReader reader)
        {
            // ReadPipeAsync reads from the PipeReader and parses incoming lines.
            while (true)
            {
                // PipeReader.ReadAsync() returns the data that was read in the form of ReadOnlySequence<byte> and
                // a bool IsCompleted that lets the reader know if the writer is done writing (EOF)
                ReadResult result = await reader.ReadAsync();

                ReadOnlySequence<byte> buffer = result.Buffer;
                SequencePosition? position;

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
        
        private Company ProcessCompanyCode(ReadOnlySequence<byte> sequence, string companyCode)
        {
            var result = new Company();

            var split = Encoding.UTF8.GetString(sequence).Split(";");

            // Console.WriteLine(split[0] + " " + split[1]);
            
            if (split[1].Equals(companyCode))
            {
                result.Name = split[0];
                
            }
            
            return result;
        }
        
    }
}