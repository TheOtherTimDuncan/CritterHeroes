/// <binding ProjectOpened='watch' />
'use strict';

var gulp = require('gulp');
var plugins = require('gulp-load-plugins')({
    pattern: ['*'],
    scope: ['devDependencies'],
    replaceString: /\bgulp[\-.]/
});

var common = {
    srcPath: 'src',
    distPath: 'dist',
    fs: require('fs'),
    path: require('path'),
    bowerBase: 'bower_components'
};

function getTask(task) {
    return require('./gulp-tasks/' + task)(gulp, plugins, common);
}

gulp.task('build-emails', getTask('emails.js'));
gulp.task('build-scripts', getTask('scripts.js'));
gulp.task('build-css', getTask('css.js'));
gulp.task('build-templates', getTask('handlebars.js'));

gulp.task('watch', ['build-scripts', 'build-css'], function () {

    gulp.watch(common.srcPath + '/js/**/*.js', ['build-scripts']);
    gulp.watch(common.srcPath + '/less/**/*.less', ['build-css']);
    gulp.watch(common.srcPath + '/js/templates/*.hb', ['build-templates']);

});
