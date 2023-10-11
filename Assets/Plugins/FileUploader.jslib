mergeInto(LibraryManager.library, {
  uploadFile: function() {
    var input = document.createElement('input');
    input.type = 'file';
    input.setAttribute('accept', '.csv');
    
    input.onchange = function(e) { 
        var file = e.target.files[0];
        var fileName = file.name;
        var reader = new FileReader();
        
        reader.readAsText(file);
        
        reader.onload = function() {
            var combinedString = fileName + "|" + reader.result;
            // 将文件名和文件内容传递给 Unity
            Module.SendMessage('YourGameObjectName', 'OnFileUploaded', combinedString);
        };
    }
    
    input.click();
  }
});
