'use strict';

customElements.define('compodoc-menu', class extends HTMLElement {
    constructor() {
        super();
        this.isNormalMode = this.getAttribute('mode') === 'normal';
    }

    connectedCallback() {
        this.render(this.isNormalMode);
    }

    render(isNormalMode) {
        let tp = lithtml.html(`
        <nav>
            <ul class="list">
                <li class="title">
                    <a href="index.html" data-type="index-link">bookingbuddy.client documentation</a>
                </li>

                <li class="divider"></li>
                ${ isNormalMode ? `<div id="book-search-input" role="search"><input type="text" placeholder="Type to search"></div>` : '' }
                <li class="chapter">
                    <a data-type="chapter-link" href="index.html"><span class="icon ion-ios-home"></span>Getting started</a>
                    <ul class="links">
                        <li class="link">
                            <a href="overview.html" data-type="chapter-link">
                                <span class="icon ion-ios-keypad"></span>Overview
                            </a>
                        </li>
                        <li class="link">
                            <a href="index.html" data-type="chapter-link">
                                <span class="icon ion-ios-paper"></span>README
                            </a>
                        </li>
                                <li class="link">
                                    <a href="dependencies.html" data-type="chapter-link">
                                        <span class="icon ion-ios-list"></span>Dependencies
                                    </a>
                                </li>
                                <li class="link">
                                    <a href="properties.html" data-type="chapter-link">
                                        <span class="icon ion-ios-apps"></span>Properties
                                    </a>
                                </li>
                    </ul>
                </li>
                    <li class="chapter modules">
                        <a data-type="chapter-link" href="modules.html">
                            <div class="menu-toggler linked" data-bs-toggle="collapse" ${ isNormalMode ?
                                'data-bs-target="#modules-links"' : 'data-bs-target="#xs-modules-links"' }>
                                <span class="icon ion-ios-archive"></span>
                                <span class="link-name">Modules</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                        </a>
                        <ul class="links collapse " ${ isNormalMode ? 'id="modules-links"' : 'id="xs-modules-links"' }>
                            <li class="link">
                                <a href="modules/AppModule.html" data-type="entity-link" >AppModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AppModule-2584e4ddf6314b22030920b6683e75ec2511c7423819d7cdfed223389c05f603a3a491b60a02f5915e46170e5b120d9813bee38ce1bd8a3b7275a0e096076ed3"' : 'data-bs-target="#xs-components-links-module-AppModule-2584e4ddf6314b22030920b6683e75ec2511c7423819d7cdfed223389c05f603a3a491b60a02f5915e46170e5b120d9813bee38ce1bd8a3b7275a0e096076ed3"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AppModule-2584e4ddf6314b22030920b6683e75ec2511c7423819d7cdfed223389c05f603a3a491b60a02f5915e46170e5b120d9813bee38ce1bd8a3b7275a0e096076ed3"' :
                                            'id="xs-components-links-module-AppModule-2584e4ddf6314b22030920b6683e75ec2511c7423819d7cdfed223389c05f603a3a491b60a02f5915e46170e5b120d9813bee38ce1bd8a3b7275a0e096076ed3"' }>
                                            <li class="link">
                                                <a href="components/AdInfoStepComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AdInfoStepComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AmenitiesStepComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AmenitiesStepComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AmenityComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AmenityComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/AppComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AppComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/BookingComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BookingComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ChatComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ChatComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FavoriteSidebarComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FavoriteSidebarComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/FavoritebarPropertyComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >FavoritebarPropertyComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/HomepageComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >HomepageComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/HomepagePropertyComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >HomepagePropertyComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/InitialStepComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >InitialStepComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LocationStepComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LocationStepComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MenuComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MenuComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PhotosStepComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PhotosStepComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ProfileComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ProfileComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PropertyAdCreateComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PropertyAdCreateComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PropertyAdRetrieveComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PropertyAdRetrieveComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-AppModule-2584e4ddf6314b22030920b6683e75ec2511c7423819d7cdfed223389c05f603a3a491b60a02f5915e46170e5b120d9813bee38ce1bd8a3b7275a0e096076ed3"' : 'data-bs-target="#xs-injectables-links-module-AppModule-2584e4ddf6314b22030920b6683e75ec2511c7423819d7cdfed223389c05f603a3a491b60a02f5915e46170e5b120d9813bee38ce1bd8a3b7275a0e096076ed3"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-AppModule-2584e4ddf6314b22030920b6683e75ec2511c7423819d7cdfed223389c05f603a3a491b60a02f5915e46170e5b120d9813bee38ce1bd8a3b7275a0e096076ed3"' :
                                        'id="xs-injectables-links-module-AppModule-2584e4ddf6314b22030920b6683e75ec2511c7423819d7cdfed223389c05f603a3a491b60a02f5915e46170e5b120d9813bee38ce1bd8a3b7275a0e096076ed3"' }>
                                        <li class="link">
                                            <a href="injectables/AuthGuard.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AuthGuard</a>
                                        </li>
                                        <li class="link">
                                            <a href="injectables/AuthorizeService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >AuthorizeService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                            <li class="link">
                                <a href="modules/AppRoutingModule.html" data-type="entity-link" >AppRoutingModule</a>
                            </li>
                            <li class="link">
                                <a href="modules/AuthModule.html" data-type="entity-link" >AuthModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AuthModule-0a4db8f19a7e916e0299426cba64bb5db34bfe7c5221d7461cf0aab5b47cd80d3a82efae6d85244eb201a511dae3d8b2cb6edd69bf9aec0614140b6f4f7a3324"' : 'data-bs-target="#xs-components-links-module-AuthModule-0a4db8f19a7e916e0299426cba64bb5db34bfe7c5221d7461cf0aab5b47cd80d3a82efae6d85244eb201a511dae3d8b2cb6edd69bf9aec0614140b6f4f7a3324"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AuthModule-0a4db8f19a7e916e0299426cba64bb5db34bfe7c5221d7461cf0aab5b47cd80d3a82efae6d85244eb201a511dae3d8b2cb6edd69bf9aec0614140b6f4f7a3324"' :
                                            'id="xs-components-links-module-AuthModule-0a4db8f19a7e916e0299426cba64bb5db34bfe7c5221d7461cf0aab5b47cd80d3a82efae6d85244eb201a511dae3d8b2cb6edd69bf9aec0614140b6f4f7a3324"' }>
                                            <li class="link">
                                                <a href="components/ConfirmEmailComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ConfirmEmailComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/GoogleComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >GoogleComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LogoutComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LogoutComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/MicrosoftComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >MicrosoftComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RecoverPwComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RecoverPwComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/RegisterComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >RegisterComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ResetPwComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ResetPwComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SignInComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SignInComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/AuxiliaryModule.html" data-type="entity-link" >AuxiliaryModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-AuxiliaryModule-ca7de8c5123dcfcf238a33d40cee1336182d95ab46a41a2e5f146b1c82c497d6f2e7f92d2697d0ad4ee050b37503290400f7a0f710f28a64c2b5286ea81bd818"' : 'data-bs-target="#xs-components-links-module-AuxiliaryModule-ca7de8c5123dcfcf238a33d40cee1336182d95ab46a41a2e5f146b1c82c497d6f2e7f92d2697d0ad4ee050b37503290400f7a0f710f28a64c2b5286ea81bd818"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-AuxiliaryModule-ca7de8c5123dcfcf238a33d40cee1336182d95ab46a41a2e5f146b1c82c497d6f2e7f92d2697d0ad4ee050b37503290400f7a0f710f28a64c2b5286ea81bd818"' :
                                            'id="xs-components-links-module-AuxiliaryModule-ca7de8c5123dcfcf238a33d40cee1336182d95ab46a41a2e5f146b1c82c497d6f2e7f92d2697d0ad4ee050b37503290400f7a0f710f28a64c2b5286ea81bd818"' }>
                                            <li class="link">
                                                <a href="components/BadRequestComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >BadRequestComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/ForbiddenComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >ForbiddenComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/LoaderComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >LoaderComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/NotfoundComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >NotfoundComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/UnauthorizedComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >UnauthorizedComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                            </li>
                            <li class="link">
                                <a href="modules/HostingModule.html" data-type="entity-link" >HostingModule</a>
                                    <li class="chapter inner">
                                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                            'data-bs-target="#components-links-module-HostingModule-f81a8adc6626addc47a963b8285b1db26534393961b076d036e422d86613e3467015ba7f3b45b9f59d38ebd5b74d77904d1ac6b445f4b340743ccf7d96d01baf"' : 'data-bs-target="#xs-components-links-module-HostingModule-f81a8adc6626addc47a963b8285b1db26534393961b076d036e422d86613e3467015ba7f3b45b9f59d38ebd5b74d77904d1ac6b445f4b340743ccf7d96d01baf"' }>
                                            <span class="icon ion-md-cog"></span>
                                            <span>Components</span>
                                            <span class="icon ion-ios-arrow-down"></span>
                                        </div>
                                        <ul class="links collapse" ${ isNormalMode ? 'id="components-links-module-HostingModule-f81a8adc6626addc47a963b8285b1db26534393961b076d036e422d86613e3467015ba7f3b45b9f59d38ebd5b74d77904d1ac6b445f4b340743ccf7d96d01baf"' :
                                            'id="xs-components-links-module-HostingModule-f81a8adc6626addc47a963b8285b1db26534393961b076d036e422d86613e3467015ba7f3b45b9f59d38ebd5b74d77904d1ac6b445f4b340743ccf7d96d01baf"' }>
                                            <li class="link">
                                                <a href="components/CalendarComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CalendarComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/CalendarPopupComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >CalendarPopupComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/HostingBookingComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >HostingBookingComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PropertyPerformanceComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PropertyPerformanceComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/PropertyPromoteComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >PropertyPromoteComponent</a>
                                            </li>
                                            <li class="link">
                                                <a href="components/SidePropertiesComponent.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >SidePropertiesComponent</a>
                                            </li>
                                        </ul>
                                    </li>
                                <li class="chapter inner">
                                    <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ?
                                        'data-bs-target="#injectables-links-module-HostingModule-f81a8adc6626addc47a963b8285b1db26534393961b076d036e422d86613e3467015ba7f3b45b9f59d38ebd5b74d77904d1ac6b445f4b340743ccf7d96d01baf"' : 'data-bs-target="#xs-injectables-links-module-HostingModule-f81a8adc6626addc47a963b8285b1db26534393961b076d036e422d86613e3467015ba7f3b45b9f59d38ebd5b74d77904d1ac6b445f4b340743ccf7d96d01baf"' }>
                                        <span class="icon ion-md-arrow-round-down"></span>
                                        <span>Injectables</span>
                                        <span class="icon ion-ios-arrow-down"></span>
                                    </div>
                                    <ul class="links collapse" ${ isNormalMode ? 'id="injectables-links-module-HostingModule-f81a8adc6626addc47a963b8285b1db26534393961b076d036e422d86613e3467015ba7f3b45b9f59d38ebd5b74d77904d1ac6b445f4b340743ccf7d96d01baf"' :
                                        'id="xs-injectables-links-module-HostingModule-f81a8adc6626addc47a963b8285b1db26534393961b076d036e422d86613e3467015ba7f3b45b9f59d38ebd5b74d77904d1ac6b445f4b340743ccf7d96d01baf"' }>
                                        <li class="link">
                                            <a href="injectables/HostingService.html" data-type="entity-link" data-context="sub-entity" data-context-id="modules" >HostingService</a>
                                        </li>
                                    </ul>
                                </li>
                            </li>
                </ul>
                </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#components-links"' :
                            'data-bs-target="#xs-components-links"' }>
                            <span class="icon ion-md-cog"></span>
                            <span>Components</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="components-links"' : 'id="xs-components-links"' }>
                            <li class="link">
                                <a href="components/HomepagePropertyComponent-1.html" data-type="entity-link" >HomepagePropertyComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/PaymentComponent.html" data-type="entity-link" >PaymentComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/SidePropertiesComponent-1.html" data-type="entity-link" >SidePropertiesComponent</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#classes-links"' :
                            'data-bs-target="#xs-classes-links"' }>
                            <span class="icon ion-ios-paper"></span>
                            <span>Classes</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="classes-links"' : 'id="xs-classes-links"' }>
                            <li class="link">
                                <a href="classes/AmenitiesHelper.html" data-type="entity-link" >AmenitiesHelper</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#injectables-links"' :
                                'data-bs-target="#xs-injectables-links"' }>
                                <span class="icon ion-md-arrow-round-down"></span>
                                <span>Injectables</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                            <ul class="links collapse " ${ isNormalMode ? 'id="injectables-links"' : 'id="xs-injectables-links"' }>
                                <li class="link">
                                    <a href="injectables/AdminRoleGuardService.html" data-type="entity-link" >AdminRoleGuardService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AuthGuard.html" data-type="entity-link" >AuthGuard</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/AuthorizeService.html" data-type="entity-link" >AuthorizeService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/BookingService.html" data-type="entity-link" >BookingService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FavoriteService.html" data-type="entity-link" >FavoriteService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/FeedbackService.html" data-type="entity-link" >FeedbackService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/HostingService.html" data-type="entity-link" >HostingService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/LandlordRoleGuardService.html" data-type="entity-link" >LandlordRoleGuardService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PaymentService.html" data-type="entity-link" >PaymentService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PropertyAdRetrieveComponent.html" data-type="entity-link" >PropertyAdRetrieveComponent</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/PropertyAdService.html" data-type="entity-link" >PropertyAdService</a>
                                </li>
                            </ul>
                        </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#interceptors-links"' :
                            'data-bs-target="#xs-interceptors-links"' }>
                            <span class="icon ion-ios-swap"></span>
                            <span>Interceptors</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="interceptors-links"' : 'id="xs-interceptors-links"' }>
                            <li class="link">
                                <a href="interceptors/AuthInterceptor.html" data-type="entity-link" >AuthInterceptor</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#interfaces-links"' :
                            'data-bs-target="#xs-interfaces-links"' }>
                            <span class="icon ion-md-information-circle-outline"></span>
                            <span>Interfaces</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? ' id="interfaces-links"' : 'id="xs-interfaces-links"' }>
                            <li class="link">
                                <a href="interfaces/AdInfo.html" data-type="entity-link" >AdInfo</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Amenity.html" data-type="entity-link" >Amenity</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApplicationUser.html" data-type="entity-link" >ApplicationUser</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/BookingOrder.html" data-type="entity-link" >BookingOrder</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/MapLocation.html" data-type="entity-link" >MapLocation</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/NewMessage.html" data-type="entity-link" >NewMessage</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Property.html" data-type="entity-link" >Property</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PropertyCreate.html" data-type="entity-link" >PropertyCreate</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PropertyMetrics.html" data-type="entity-link" >PropertyMetrics</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/PropertyPromote.html" data-type="entity-link" >PropertyPromote</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Rating.html" data-type="entity-link" >Rating</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UserDto.html" data-type="entity-link" >UserDto</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/UserInfo.html" data-type="entity-link" >UserInfo</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#miscellaneous-links"'
                            : 'data-bs-target="#xs-miscellaneous-links"' }>
                            <span class="icon ion-ios-cube"></span>
                            <span>Miscellaneous</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="miscellaneous-links"' : 'id="xs-miscellaneous-links"' }>
                            <li class="link">
                                <a href="miscellaneous/enumerations.html" data-type="entity-link">Enums</a>
                            </li>
                            <li class="link">
                                <a href="miscellaneous/variables.html" data-type="entity-link">Variables</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <a data-type="chapter-link" href="routes.html"><span class="icon ion-ios-git-branch"></span>Routes</a>
                        </li>
                    <li class="chapter">
                        <a data-type="chapter-link" href="coverage.html"><span class="icon ion-ios-stats"></span>Documentation coverage</a>
                    </li>
                    <li class="divider"></li>
                    <li class="copyright">
                        Documentation generated using <a href="https://compodoc.app/" target="_blank" rel="noopener noreferrer">
                            <img data-src="images/compodoc-vectorise.png" class="img-responsive" data-type="compodoc-logo">
                        </a>
                    </li>
            </ul>
        </nav>
        `);
        this.innerHTML = tp.strings;
    }
});