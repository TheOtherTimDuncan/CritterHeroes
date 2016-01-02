'use strict';

var lessPluginAutoPrefix = require('less-plugin-autoprefix');

module.exports = function (gulp, plugins, common) {

    var distCss = common.distPath + '/css';
    var distFonts = common.distPath + '/fonts';
    var distImages = common.distPath + '/images';

    var srcLess = common.srcPath + '/css';
    var srcImages = common.srcPath + '/images';

    var libBootstrap = srcLess + '/bootstrap';
    var libAwesome = srcLess + '/fontawesome';

    gulp.task('clean-css', function () {
        return plugins.del([distCss + '/**/*', distFonts + '/**/*', distImages + '/**/*', libBootstrap + '/**/*', libAwesome + '/**/*', './versioned-css.json'], { debug: true });
    });

    gulp.task('copy-images', ['clean-css'], function () {

        return gulp.src(srcImages + '/**/*.*')
            .pipe(gulp.dest(distImages));

    });

    gulp.task('copy-bootstrap', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/bootstrap/less/**/*.less')
            .pipe(gulp.dest(libBootstrap));

    });

    gulp.task('copy-fontawesome', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/font-awesome/less/**/*.less')
            .pipe(gulp.dest(libAwesome));

    });

    gulp.task('copy-fontawesome-fonts', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/font-awesome/fonts/**/')
            .pipe(gulp.dest(distFonts));

    });

    gulp.task('copy-bootswatch', ['clean-css', 'copy-bootstrap'], function () {

        return gulp.src(common.bowerBase + '/bootswatch/yeti/*.less')
            .pipe(gulp.dest(libBootstrap));

    });

    gulp.task('copy-bootstrap-fonts', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/bootstrap/dist/fonts/**/')
            .pipe(gulp.dest(distFonts));

    });

    gulp.task('normalize.css', ['clean-css'], function () {

        return gulp.src(common.bowerBase + '/normalize-css/normalize.css')
            .pipe(gulp.dest(distCss));

    });

    gulp.task('app-less', ['clean-css', 'copy-bootstrap', 'copy-bootswatch', 'copy-fontawesome'], function () {

        var autoprefix = new lessPluginAutoPrefix({ browsers: ['last 2 versions'] });

        return gulp.src(srcLess + '/*.less')
            .pipe(plugins.less({ plugins: [autoprefix] }))
            .pipe(gulp.dest(srcLess))
            .pipe(gulp.dest(distCss));

    });

    gulp.task('version-css', ['clean-css', 'normalize.css', 'app-less'], function () {

        var autoprefix = new lessPluginAutoPrefix({ browsers: ['last 2 versions'] });

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

    return ['clean-css', 'copy-images', 'copy-bootstrap', 'copy-bootswatch', 'copy-bootstrap-fonts', 'normalize.css', 'copy-fontawesome', 'copy-fontawesome-fonts', 'app-less', 'version-css'];

};
