$(document).ready(() => {
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
        const colorModel = $('#color-model').val();

        $.ajax({
            url: `/image/compareImages?fileName1=${firstFileName}&fileName2=${secondFileName}&tolerance=${tolerance}&maxDifferences=${maxDifferences}&colorModel=${colorModel}`,
            beforeSend: () => {
                $('#result').attr('src', '');
                $('#compare-loader').removeClass('hidden');
                $('#compare').attr('disabled', 'disabled');
            },
            success: data => {
                if (data.isSuccess) {
                    $('#result').attr('src', `/file/preview?fileName=${data.resultFileName}`);
                } else {
                    $.jGrowl(data.message, { header: 'Error' });
                }
            },
            error: () => {
                $.jGrowl("Server error", { header: 'Error' });
            },
            complete: () => {
                $('#compare-loader').addClass('hidden');
                $('#compare').attr('disabled', false);
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

        $("#first-file").val(null);
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

        $("#second-file").val(null);
    });

    $('#second-preview').on('load', () => {
        setUploadStatus('second-image-container', 'loaded');
    });

    $('#tolerance').on('input', e => {
        $('#tolerance-value').text(e.target.value);
    });

    $('#first-image-container > span').click(() => {
        setUploadStatus('first-image-container', '');
        $('#first-preview').attr('src', '');
        firstFileName = '';
    });

    $('#second-image-container > span').click(() => {
        setUploadStatus('second-image-container', '');
        $('#second-preview').attr('src', '');
        secondFileName = '';
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
