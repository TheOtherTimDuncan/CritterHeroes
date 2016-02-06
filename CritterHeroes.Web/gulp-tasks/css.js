'use strict';

module.exports = function (gulp, plugins, common) {

    var distCss = common.distPath + '/css';
    var distFonts = common.distPath + '/fonts';
    var distImages = common.distPath + '/images';

    var srcSass = common.srcPath + '/css';
    var srcImages = common.srcPath + '/images';

    var libAwesome = srcSass + '/fontawesome';

    gulp.task('clean-css', function () {
        return plugins.del([distCss + '/*.*', distFonts + '/*.*', distImages + '/*.*', libAwesome + '/*.*', './versioned-css.json', '!site.css']);
    });

    gulp.task('copy-images', ['clean-css'], function () {

        return gulp.src(srcImages + '/**/*.*')
            .pipe(gulp.dest(distImages));

    });

    gulp.task('copy-fontawesome', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/font-awesome/scss/*.scss')
            .pipe(gulp.dest(libAwesome));

    });

    gulp.task('copy-fontawesome-fonts', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/font-awesome/fonts/*.*')
            .pipe(gulp.dest(distFonts));

    });

    gulp.task('normalize.css', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/normalize-css/normalize.css')
            .pipe(gulp.dest(distCss));

    });

    gulp.task('app-sass', ['clean-css', 'copy-fontawesome', 'copy-fontawesome-fonts'], function () {

        return gulp.src(srcSass + '/*.scss')
            .pipe(plugins.sass().on('error', plugins.sass.logError))
            .pipe(plugins.autoprefixer({ browsers: ['last 2 versions'] }))
            .pipe(gulp.dest(srcSass))
            .pipe(gulp.dest(distCss));

    });

    gulp.task('version-css', ['clean-css', 'normalize.css', 'app-sass'], function () {

        return gulp.src(distCss + '/*.css', { base: common.distPath })
            .pipe(plugins.sourcemaps.init())
            .pipe(plugins.minifyCss())
            .pipe(plugins.rename({ extname: '.min.css' }))
            .pipe(plugins.rev())
            .pipe(plugins.sourcemaps.write('.'))
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.rev.manifest("versioned-css.json"))
            .pipe(gulp.dest('./'));

    });

    return ['clean-css', 'copy-images', 'normalize.css', 'copy-fontawesome', 'copy-fontawesome-fonts', 'app-sass', 'version-css'];

};
