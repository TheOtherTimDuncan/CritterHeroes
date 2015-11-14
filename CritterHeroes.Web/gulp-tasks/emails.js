'use strict';

module.exports = function (gulp, plugins, common) {

    var srcEmails = common.srcPath + '/Emails';
    var targetEmails = 'Areas/Emails';
    var targetExamples = '../.vs/Emails';

    function getFolders(dir) {
        return common.fs.readdirSync(dir)
            .filter(function (file) {
                return common.fs.statSync(common.path.join(dir, file)).isDirectory();
            });
    }

    gulp.task('clean-emails', function () {
        return plugins.del([targetEmails, targetExamples, '!' + targetEmails], { force: true });
    });

    gulp.task('copy-emails', ['clean-emails'], function () {

        return gulp.src(srcEmails + '/**/*.txt')
            .pipe(gulp.dest(targetEmails));

    });

    gulp.task('handlebars-examples', ['clean-emails'], function () {

        var folders = getFolders(srcEmails);

        var txtTasks = folders.map(function (folder) {

            var example = common.fs.readFileSync(srcEmails + '/' + folder + '/example.json', 'utf8');
            var data = JSON.parse(example);

            return gulp.src(common.path.join(srcEmails, folder, '/**/*.txt'))
                .pipe(plugins.compileHandlebars(data))
                .pipe(gulp.dest(targetExamples + '/' + folder));

        });

        var htmlTasks = folders.map(function (folder) {

            var example = common.fs.readFileSync(srcEmails + '/' + folder + '/example.json', 'utf8');
            var data = JSON.parse(example);

            return gulp.src(common.path.join(srcEmails, folder, '/**/*.html'))
                .pipe(plugins.compileHandlebars(data))
                .pipe(plugins.inlineCss())
                .pipe(gulp.dest(targetExamples + '/' + folder));

        });

        return plugins.mergeStream(txtTasks, htmlTasks);

    });

    gulp.task('inline-emails', ['clean-emails'], function () {

        return gulp.src(srcEmails + '/**/*.html')
            .pipe(plugins.inlineCss())
            .pipe(plugins.htmlmin({ collapseWhitespace: true }))
            .pipe(gulp.dest(targetEmails));

    });

    return ['clean-emails', 'handlebars-examples', 'inline-emails', 'copy-emails'];
}
