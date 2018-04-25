function show(input) {
    var files = input.files;
    var ul = $("#Pictures_container");
    ul.empty();

    $.each(files, function (i, file) {

        if (!file.type.match('image.*')) {
            return true;//works like continue in a normal loop
        }
        var reader = new FileReader();

        reader.onload = function (e) {
            var li = '<li> <img class="img-thumbnail" src="' + e.target.result + '" title="'
                + escape(file.name) + '" ></li > ';
            ul.append(li);
        };
        reader.readAsDataURL(file);
    });
}