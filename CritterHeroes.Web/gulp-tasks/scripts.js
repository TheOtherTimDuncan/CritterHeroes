'use strict';

module.exports = function (gulp, plugins, common) {

    var distScripts = common.distPath + '/js';
    var libScripts = common.srcPath + '/lib';
    var appScripts = common.srcPath + '/js';

    gulp.task('clean-scripts', function () {
        return plugins.del([distScripts + '/*.*', common.distPath + '/lib/*.*', libScripts + '/*.*', './versioned-js.json']);
    });

    gulp.task('copy-scripts-src', ['clean-scripts'], function () {

        var sources = [
            common.bowerBase + '/jquery/dist/jquery.js',
            common.bowerBase + '/jquery/dist/jquery.min.js',
            common.bowerBase + '/jquery/dist/jquery.min.map',
            common.bowerBase + '/jquery-validation/dist/jquery.validate.js',
            common.bowerBase + '/jquery-validation/dist/jquery.validate.min.js',
            common.bowerBase + '/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js',
            common.bowerBase + '/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js',
            common.bowerBase + '/dropzone//dist/dropzone.js',
            common.bowerBase + '/dropzone/dist/min/dropzone.min.js',
            common.bowerBase + '/bootstrap/dist/js/bootstrap.js',
            common.bowerBase + '/bootstrap/dist/js/bootstrap.min.js',
            'node_modules/handlebars/dist/handlebars.runtime.js',
            'node_modules/handlebars/dist/handlebars.runtime.min.js'
        ];

        return gulp.src(sources, { base: common.bowerBase })
            .pipe(plugins.flatten())
            .pipe(gulp.dest(libScripts));

    });

    gulp.task('copy-scripts-dist', ['clean-scripts', 'copy-scripts-src'], function () {

        return gulp.src([libScripts + '/*.js', '!' + libScripts + '/*.min.js'], { base: common.srcPath })
            .pipe(gulp.dest(common.distPath));

    });

    gulp.task('copy-scripts-dist-min', ['clean-scripts', 'copy-scripts-src', 'copy-scripts-dist'], function () {

        return gulp.src([libScripts + '/*.min.js'], { base: common.srcPath })
            .pipe(plugins.plumber({
                errorHandler: function (err) {
                    console.log(err);
                    this.emit('end');
                }
            }))
            .pipe(plugins.rev())
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.rev.manifest({ path: "versioned-lib.json" }))
            .pipe(gulp.dest('./'));

    });

    gulp.task('app-bundle', ['clean-scripts'], function () {

        var sources = [
            appScripts + '/bundled/ch.common.js',
            appScripts + '/bundled/ch.pubsub.js',
            appScripts + '/bundled/ch.data.js',
            appScripts + '/bundled/ch.history.js',
            appScripts + '/bundled/ch.pagify.js',
            appScripts + '/bundled/ch.menu.js',
            appScripts + '/bundled/ch.modal.js'
        ];

        return gulp.src(sources)
            .pipe(plugins.plumber({
                errorHandler: function (err) {
                    console.log(err);
                    this.emit('end');
                }
            }))
            .pipe(plugins.concat('js/cheroes.js'))
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.sourcemaps.init())
            .pipe(plugins.uglify())
            .pipe(plugins.rename({ extname: '.min.js' }))
            .pipe(plugins.rev())
            .pipe(plugins.sourcemaps.write('.'))
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.rev.manifest({ path: "versioned-js.json", merge: true }))
            .pipe(gulp.dest('./'));


    });

    gulp.task('app-scripts', ['clean-scripts'], function () {

        return gulp.src(appScripts + '/*.js', { base: common.srcPath })
            .pipe(plugins.plumber({
                errorHandler: function (err) {
                    console.log(err);
                    this.emit('end');
                }
            }))
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.sourcemaps.init())
            .pipe(plugins.uglify())
            .pipe(plugins.rename({ extname: '.min.js' }))
            .pipe(plugins.rev())
            .pipe(plugins.sourcemaps.write('.'))
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.rev.manifest("versioned-js.json", { merge: true }))
            .pipe(gulp.dest('./'));

    });

    return ['clean-scripts', 'copy-scripts-src', 'copy-scripts-dist', 'copy-scripts-dist-min', 'app-bundle', 'app-scripts'];

};
