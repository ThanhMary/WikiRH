
function clickLink(line, id) {
    var q = {};
    var extented = document.getElementById('btnConsult_' + line).ariaExpanded;
   
    if (extented == null || extented == "false") { 
        q.Id_quest = id;
        $.ajax({
            type: 'POST',
            dataType: 'JSON',
            url: `${ROOT}Questions/UpdateCount`,
            data: { question: q } // question doit etre le nom paramettre de l'objet du cote de controler
        });
    }
}




