function show(input) {
    var files = input.files;
    var ul = $("#Pictures_container");
    ul.empty();
    console.log('lenght', files.length);
    var sizeError = false;

    $.each(files, function (i, file) {
        console.log("10485760");
        if (file.size > 10485760) {
            sizeError = true;
        }
    });

    if (input.files.length > 10) {
        $(input).val('');
        alert('Max allowed picture count is 10.');
    } else if (sizeError) {
        $(input).val('');
        alert('Image cannot exceeded 10Mb');
    }
    else {
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
}