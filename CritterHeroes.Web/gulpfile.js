'use strict';

var gulp = require('gulp');
var plugins = require('gulp-load-plugins')({
    pattern: ['*'],
    replaceString: /\bgulp[\-.]/
});

var common = {
    srcPath: 'src',
    fs: require('fs'),
    path: require('path')
}

function getTask(task) {
    return require('./gulp-tasks/' + task)(gulp, plugins, common);
}

gulp.task('build-emails', getTask('emails.js'));
