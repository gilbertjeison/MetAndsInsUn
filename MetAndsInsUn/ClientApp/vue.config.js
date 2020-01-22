module.exports = {
    devServer: {
        // When running in IISExpress, the env variable wont be provided. Hardcode it here based on your launchSettings.json
        proxy: 'http://localhost:5000'
      },
 configureWebpack: {
//    // The URL where the .Net Core app will be listening.
//    //    See https://cli.vuejs.org/config/#devserver-proxy and https://webpack.js.org/configuration/dev-server#devserverproxy
//    // Instead of hardcoding something lile https://localhost:5001/,
//    // read the ASPNET_URL environment variable, injected by VueDevelopmentServerMiddleware
//    devServer: {
//      // When running in IISExpress, the env variable wont be provided. Hardcode it here based on your launchSettings.json
//      proxy: process.env.ASPNET_URL || 'http://localhost:5000'
//    },
//    // Use source map for debugging in VS and VS Code
   devtool: 'source-map',
//    // Breakpoints in VS and VSCode wont work since the source maps conside clien-app the project root, rather than its parent folder
//    output: {
//      devtoolModuleFilenameTemplate: info => {
//        const resourcePath = info.resourcePath.replace('./src', './ClientApp/src')
//        return `webpack:///${resourcePath}?${info.loaders}`
//      }
//    }
 },
chainWebpack: (config) => {
    config.plugin('define').tap((definitions) => {
      definitions[0]['process.env']['ASSET_PATH'] = JSON.stringify(process.env.NODE_ENV === 'production'
      ? '/metins/'
      : '/');
      return definitions;
    });
  },
publicPath: process.env.NODE_ENV === 'production'
    ? '/metins/'
    : '/'
}
