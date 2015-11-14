'use strict';

var gulp = require('gulp');
var inlineCss = require('gulp-inline-css');
var fs = require('fs');
var path = require('path');
var del = require('del');
var handlebars = require('gulp-compile-handlebars');
var htmlmin = require('gulp-htmlmin');
var merge = require('merge-stream');

var srcPath = 'src';

var srcEmails = srcPath + '/Emails';
var targetEmails = 'Areas/Emails';
var targetExamples = '../.vs/Emails';

function getFolders(dir) {
    return fs.readdirSync(dir)
        .filter(function (file) {
            return fs.statSync(path.join(dir, file)).isDirectory();
        });
}

gulp.task('clean-emails', function () {
    del(targetEmails);
    del(targetExamples);
});

gulp.task('copy-emails', ['clean-emails', 'inline-emails'], function () {

    return gulp.src(srcEmails + '/**/*.txt')
        .pipe(inlineCss())
        .pipe(gulp.dest(targetEmails));

});

gulp.task('handlebars-examples', ['clean-emails', 'inline-emails'], function () {

    var folders = getFolders(srcEmails);

    var txtTasks = folders.map(function (folder) {

        var example = fs.readFileSync(srcEmails + '/' + folder + '/example.json', 'utf8');
        var data = JSON.parse(example);

        return gulp.src(path.join(srcEmails, folder, '/**/*.txt'))
            .pipe(handlebars(data))
            .pipe(inlineCss())
            .pipe(gulp.dest(targetExamples + '/' + folder));

    });

    var htmlTasks = folders.map(function (folder) {

        var example = fs.readFileSync(srcEmails + '/' + folder + '/example.json', 'utf8');
        var data = JSON.parse(example);

        return gulp.src(path.join(srcEmails, folder, '/**/*.html'))
            .pipe(handlebars(data))
            .pipe(inlineCss())
            .pipe(gulp.dest(targetExamples + '/' + folder));

    });

    return merge(txtTasks, htmlTasks);

});

gulp.task('inline-emails', ['clean-emails'], function () {

    return gulp.src(srcEmails + '/**/*.html')
        .pipe(inlineCss())
        .pipe(htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest(targetEmails));

});

gulp.task('build-emails', ['clean-emails', 'handlebars-examples', 'inline-emails', 'copy-emails']);
