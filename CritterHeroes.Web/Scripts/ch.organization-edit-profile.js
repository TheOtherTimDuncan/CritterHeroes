(function (cheroes, $, Dropzone) {

    'use strict';

    var logo = $('#logo');
    var progress = $('.progress');
    var progressBar = $('.progress-bar');
    var thumbMsg = $('#thumb-msg');
    var logoError = $('#logo-error');

    var frm = $('#edit-profile').submit(function (event) {

        // If no logo uploaded then allow the form to post normally
        if (dz.getAcceptedFiles().length === 0) {
            return true;
        }

        // Otherwise let dropzone do the ajax upload
        event.preventDefault();

        // Add the form data for dropzone
        var data = frm.serializeArray();
        $.each(data, function (index, item) {
            dz.options.params[item.name] = item.value;
        });

        // Start the upload
        dz.enqueueFiles(dz.getAcceptedFiles())
    });

    var changeLogo = $('#change-logo').click(function () {
        logoError.text('');
        dz.removeAllFiles();
    });

    var dz = new Dropzone('#logo', {

        url: $('#edit-profile').prop('action'),
        thumbnailWidth: 140,
        thumbnailHeight: 140,
        autoQueue: false,
        uploadMultiple: false,
        clickable: '#change-logo',
        paramName: 'LogoFile',
        maxFiles: 1,
        acceptedFiles: 'image/*',
        dictInvalidFileType: 'Please upload only an image for the logo.',
        dictMaxFilesExceeded: 'Please upload only one image.',

        thumbnail: function (file, dataUrl) {
            logo.attr('src', dataUrl);
            changeLogo.show();
            thumbMsg.hide();
        },

        uploadprogress: function (file, progress, bytesSent) {
            progressBar.width(progress + '%');
        }

    });

    dz.on('addedfile', function (file) {
        changeLogo.hide();
        thumbMsg.show();
    });

    dz.on('error', function (file, message, xhr) {
        logoError.text(message);
        changeLogo.show();
        thumbMsg.hide();
    });

    dz.on('success', function (file, response) {
        window.location.href = response.Url;
    });

}(this.cheroes = this.cheroes || {}, jQuery, Dropzone));