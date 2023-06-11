package com.pauloreis.catalog.admin.application.category.create;

import com.pauloreis.catalog.admin.application.UseCase;
import com.pauloreis.catalog.admin.domain.validation.handler.Notification;

import io.vavr.control.Either;

public abstract class CreateCategoryUseCase
    extends UseCase<CreateCategoryCommand, Either<Notification, CreateCategoryOutput>> {

}
