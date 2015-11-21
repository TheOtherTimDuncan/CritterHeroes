/// <binding ProjectOpened='watch' />
'use strict';

var gulp = require('gulp');
var plugins = require('gulp-load-plugins')({
    pattern: ['*'],
    replaceString: /\bgulp[\-.]/
});

var common = {
    srcPath: 'src',
    distPath: 'dist',
    fs: require('fs'),
    path: require('path')
}

function getTask(task) {
    return require('./gulp-tasks/' + task)(gulp, plugins, common);
}

gulp.task('build-emails', getTask('emails.js'));
gulp.task('build-scripts', getTask('scripts.js'));

gulp.task('watch', ['build-scripts'], function () {

    gulp.watch(common.srcPath + '/js/**/*.js', ['build-scripts']);

});
