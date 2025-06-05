Time Entry Take Home Challenge

This TimeEntries project is a super basic app that displays time entries from 
the included CSV files (AppData folder) and allows the user to write time 
entries to the TimeEntries.csv file. 


SET UP

The project is fairly self contained since the data is all stored in the 
included csv files. The only thing I ran into was a Windows warning regarding
normalization of the data in the csv when adding new entries. I just clicked
'No' in the prompt and the application works as expected after that.s


APPROACH

I started the project with the available MVC template provided by Visual Studio.
I decided to go this route rather than creating a blank project from scratch 
mostly to refamiliarize myself on how MVC projects are structured. It served as a great
refresher and starting point.

The System.IO.File class is being used to read data from the csv files as well
as to insert data into the into them. It was very simple and only required 
parsing through/concatenating the data required for the time entries. 

I'm using a ViewData dictionary mostly because I started small and simple, but 
later into the project I realized this was the incorrect approach. A ViewModel
would've been infinitely better.

Lastly, I'm using bootstrap and some css for the styling and components in the 
Index view. 


IMPROVEMENTS

There are many improvements I think could be made to this simple app, but for
the sake of brevity I'll only list a few.

One of the biggest improvement I would make is using a ViewModel rather than
a ViewData dictionary. In retrospect, it would've made a lot of things much
easier if I'd done that from the beginning.

Another area for potential improvement is the approach used for reading/writing
data from/to the csv files. I'm sure there are safer and more efficient ways to
accomplish this rather than using the File class.

The last thing I'd improve is the validation on the modal form. While the validation
works, it's not bulletproof. Additionally, the re-rendering of the modal is not
the most fluid.