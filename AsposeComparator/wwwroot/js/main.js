﻿$(document).ready(() => {
    let firstFileName;
    let secondFileName;

    $('#compare').click(() => {
        if (!firstFileName || !secondFileName) {
            $.jGrowl("Please chose files", { header: 'Error' });
            return;
        }

        const maxDifferences = Number($('#max-differences').val());

        if (isNaN(maxDifferences) || maxDifferences < 0) {
            $.jGrowl("Max differences value must be a number greater than or equal to 0", { header: 'Error' });
            return;
        }

        const tolerance = $('#tolerance').val();

        $.ajax({
            url: `/image/compareImages?fileName1=${firstFileName}&fileName2=${secondFileName}&tolerance=${tolerance}&maxDifferences=${maxDifferences}`,
            success: data => {
                $('#result').attr('src', `/file/preview?fileName=${data.resultFileName}`);
            },
            error: () => {
                $.jGrowl("Server error", { header: 'Error' });
            }
        })
    });

    $('#open-first-file').click(() => {
        $('#first-file').trigger('click');
    });

    $('#open-second-file').click(() => {
        $('#second-file').trigger('click');
    });

    $('#first-file').on('change', e => {
        if (!e.target.files[0]) { return; }

        if (e.target.files[0].size / 1024 / 1024 > 50) {
            $.jGrowl("File larger than 50 Mb", { header: 'Error' });
            return;
        }

        const formData = new FormData();
        formData.append('file', e.target.files[0]);
        $.ajax({
            url: '/file/upload',
            type: 'post',
            processData: false,
            contentType: false,
            data: formData,
            beforeSend: () => setUploadStatus('first-image-container', 'loading'),
            success: data => {
                firstFileName = data.fileName;
                $('#first-preview').attr('src', `/file/preview?fileName=${firstFileName}`);
            },
            error: () => {
                setUploadStatus('first-image-container', '');
                $.jGrowl("Image not uploaded", { header: 'Error' });
            }
        });

        $("#file").val(null);
    });

    $('#first-preview').on('load', () => {
        setUploadStatus('first-image-container', 'loaded');
    });

    $('#second-file').on('change', e => {
        if (!e.target.files[0]) { return; }

        if (e.target.files[0].size / 1024 / 1024 > 50) {
            $.jGrowl("File larger than 50 Mb", { header: 'Error' });
            return;
        }

        const formData = new FormData();
        formData.append('file', e.target.files[0]);
        $.ajax({
            url: '/file/upload',
            type: 'post',
            processData: false,
            contentType: false,
            data: formData,
            beforeSend: () => setUploadStatus('second-image-container', 'loading'),
            success: data => {
                secondFileName = data.fileName;
                $('#second-preview').attr('src', `/file/preview?fileName=${secondFileName}`);
            },
            error: () => {
                setUploadStatus('second-image-container', '');
                $.jGrowl("Image not uploaded", { header: 'Error' });
            }
        });

        $("#file").val(null);
    });

    $('#second-preview').on('load', () => {
        setUploadStatus('second-image-container', 'loaded');
    });

    function setUploadStatus(id, status) {
        const element = $(`#${id}`);
        if (status === 'loading') {
            element.removeClass('loaded');
            element.addClass('loading');
        } else if (status === 'loaded') {
            element.addClass('loaded');
            element.removeClass('loading');
        } else {
            element.removeClass('loaded');
            element.removeClass('loading');
        }
    };
});
