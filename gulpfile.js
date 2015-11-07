'use strict';

var gulp = require('gulp');
var inlineCss = require('gulp-inline-css');
var fs = require('fs');
var path = require('path');
var del = require('del');
var glob = require('glob');
var handlebars = require('gulp-compile-handlebars');
var htmlmin = require('gulp-htmlmin');
var merge = require('merge-stream');

var emailsPath = 'Emails';
var buildPath = 'CritterHeroes.Web/Areas/Emails';
var examplesPath = '.vs/Emails';

function getFolders(dir) {
    return fs.readdirSync(dir)
        .filter(function (file) {
            return fs.statSync(path.join(dir, file)).isDirectory();
        });
}

gulp.task('clean', function () {
    del(buildPath);
    del(examplesPath);
});

gulp.task('copy', ['clean', 'inline-emails'], function () {

    return gulp.src(emailsPath + '/**/*.txt')
        .pipe(inlineCss())
        .pipe(gulp.dest(buildPath));

});

gulp.task('handlebars-examples', ['clean', 'inline-emails'], function () {

    var folders = getFolders(emailsPath);

    var txtTasks = folders.map(function (folder) {

        var example = fs.readFileSync(emailsPath + '/' + folder + '/example.json', 'utf8');
        var data = JSON.parse(example);

        return gulp.src(path.join(emailsPath, folder, '/**/*.txt'))
            .pipe(handlebars(data))
            .pipe(inlineCss())
            .pipe(gulp.dest(examplesPath + '/' + folder));

    });

    var htmlTasks = folders.map(function (folder) {

        var example = fs.readFileSync(emailsPath + '/' + folder + '/example.json', 'utf8');
        var data = JSON.parse(example);

        return gulp.src(path.join(emailsPath, folder, '/**/*.html'))
            .pipe(handlebars(data))
            .pipe(inlineCss())
            .pipe(gulp.dest(examplesPath + '/' + folder));

    });

    return merge(txtTasks, htmlTasks);

});

gulp.task('inline-emails', ['clean'], function () {

    return gulp.src(emailsPath + '/**/*.html')
        .pipe(inlineCss())
        .pipe(htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest(buildPath));

});

gulp.task('build', ['clean', 'handlebars-examples', 'inline-emails', 'copy']);
