'use strict';

var gulp = require('gulp');
var inlineCss = require('gulp-inline-css');
var fs = require('fs');
var path = require('path');
var del = require('del');
var glob = require('glob');
var handlebars = require('gulp-compile-handlebars');
var htmlmin = require('gulp-htmlmin');

var emailsPath = 'Emails';
var buildPath = 'CritterHeroes.Web/Areas/Emails';
var examplesPath = '.vs/Emails';

function getExamples(callback) {

    return glob(emailsPath + '/**/*.json', function (err, files) {

        if (err) {
            return console.error(err);
        }

        return files.map(callback);

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

gulp.task('handlebars-txt', ['clean'], function () {

    var tasks = getExamples(function (example) {

        return gulp.src(emailsPath + '/**/*.txt')
            .pipe(handlebars(require('./' + example)))
            .pipe(gulp.dest(examplesPath));

    });

    return tasks;

});

gulp.task('handlebars-html', ['clean'], function () {

    var tasks = getExamples(function (example) {

        return gulp.src(emailsPath + '/**/*.html')
            .pipe(handlebars(require('./' + example)))
            .pipe(inlineCss())
            .pipe(gulp.dest(examplesPath));

    });

    return tasks;

});

gulp.task('inline-emails', ['clean'], function () {

    return gulp.src(emailsPath + '/**/*.html')
        .pipe(inlineCss())
        .pipe(htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest(buildPath));

});

gulp.task('build', ['clean', 'handlebars-txt', 'handlebars-html', 'inline-emails', 'copy']);
