import { Body, Controller, Get, Param, Patch, Post } from '@nestjs/common';
import { ShareTodoService } from '../services/share-todo.service';

@Controller('share-todo')
export class ShareTodoController {
  constructor(private readonly shareTodoService: ShareTodoService) {}

  @Post('share')
  async shareTodo(
    @Body() shareData: { todoId: number; userId: number; permission: string },
  ) {
    return this.shareTodoService.shareTodoWithUser(
      shareData.todoId,
      shareData.userId,
      shareData.permission,
    );
  }

  @Get('user/:userId')
  async findSharedTodos(@Param('userId') userId: number) {
    return this.shareTodoService.findSharedTodosByUser(userId);
  }

  @Patch(':shareId/permission')
  async updatePermission(
    @Param('shareId') shareId: number,
    @Body() permissionData: { permission: string },
  ) {
    return this.shareTodoService.updatePermissions(
      shareId,
      permissionData.permission,
    );
  }
}
